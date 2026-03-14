using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;

namespace SkanderNET
{
    public class Portal : IDisposable
    {
        public readonly IUsbDevice Device;
        private UsbContext usbContext;
        private readonly UsbEndpointReader _reader;
        private readonly Queue<byte[]> _writeQueue = new Queue<byte[]>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly PortalSlot[] _slots = new PortalSlot[16];
        private bool _activated;
        private readonly Task _workerTask;
        /// <summary>
        /// Invoked when a Skylander is initially placed and its first sector containing character info is processed.
        /// </summary>
        public event Action<int, Skylander> OnSkylanderPlaced;
        /// <summary>
        /// Invoked once the active data area is fully parsed and data can be read/written.
        /// </summary>
        public event Action<int, Skylander> OnSkylanderProcessed;
        /// <summary>
        /// Invoked when a Skylander is removed from the portal.
        /// </summary>
        public event Action<int, Skylander> OnSkylanderRemoved;
        /// <summary>
        /// Invoked when <see cref="Skylander.Save"/> is called on a <see cref="Skylander"/>.
        /// </summary>
        public event Action<int, Skylander> OnSkylanderSaved;
        /// <summary>
        /// Invoked when the portal is activated and ready to being receiving queries.
        /// </summary>
        public event Action OnReady;
        /// <summary>
        /// Invoked when the portal throws an exception.
        /// </summary>
        public event Action<Exception> OnError;

        internal Portal(IUsbDevice device, UsbContext context)
        {
            usbContext = context; // Hold to prevent disposal before device disposal
            Device = device;
            device.Open();
            device.SetConfiguration(1);
            device.ClaimInterface(0);
            _reader = Device.OpenEndpointReader(ReadEndpointID.Ep01, 0x20, EndpointType.Interrupt);
            for (var i = 0; i < 0x10; i++)
                _slots[i] = new PortalSlot { Index = i };
            _workerTask = Task.Run(() => Work(_cts.Token));
        }

        /// <summary>
        /// Begins initial communication with the <see cref="Portal"/>
        /// </summary>
        public void Activate() => SendReady();
        
        private void ParseResponse(byte[] data)
        {
            var responseId = (char)data[0];
            switch (responseId)
            {
                case 'S':
                    HandleStatus(data);
                    break;
                case 'R':
                    HandleReady(data);
                    break;
                case 'A':
                    HandleActivate(data);
                    break;
                case 'Q':
                    HandleQuery(data);
                    break;
            }
        }
        
        private void HandleReady(byte[] data)
        {
            SendActivate();
        }

        private void HandleActivate(byte[] data)
        {
            _activated = true;
            OnReady?.Invoke();
        }
        
        private void HandleQuery(byte[] data)
        {
            var slotIndex = (uint)(data[1] - 0x10);
            uint blockIndex = data[2];
            if (slotIndex >= _slots.Length)
                return;
            var slot = _slots[slotIndex];
            if (slot.PendingBlock == blockIndex)
            {
                slot.PendingBlock = uint.MaxValue;
                slot.RetryCount = 0;
            }
            if (slot.CurrentSkylander == null && blockIndex == 0)
                slot.CurrentSkylander = new Skylander(this, slot.Index);

            if (slot.CurrentSkylander == null)
                return;

            var blockData = new byte[16];
            Array.Copy(data, 3, blockData, 0, 16);
            slot.CurrentSkylander.HandleBlock(blockIndex, blockData);
        }

        private void HandleStatus(byte[] data)
        {
            if (!_activated)
                return;
            var statusMask = BitConverter.ToUInt32(data, 1);
            foreach (var slot in _slots)
            {
                slot.Status = (int)(statusMask >> (slot.Index * 2)) & 3;
                switch (slot.Status)
                {
                    case 2:
                        if (slot.CurrentSkylander == null)
                            continue;
                        OnSkylanderRemoved?.Invoke(slot.Index, slot.CurrentSkylander);
                        slot.CurrentSkylander = null;
                        slot.Status = 0;
                        continue;
                    case 3:
                        SendQuery(slot.Index, 0);
                        break;
                }
            }
        }

        private void SendReady()
        {
            if (Device == null)
                return;
            var buffer = new byte[32];
            buffer[0] = (byte)'R';
            lock (_writeQueue)
                _writeQueue.Enqueue(buffer);
        }

        private void SendActivate()
        {
            if (Device == null)
                return;
            var buffer = new byte[32];
            buffer[0] = (byte)'A';
            buffer[1] = 0x1;
            lock (_writeQueue)
                _writeQueue.Enqueue(buffer);
        }
        
        internal void SendQuery(int slot, uint block)
        {
            var buffer = new byte[32];
            buffer[0] = (byte)'Q';
            buffer[1] = (byte)(0x10 + slot);
            buffer[2] = (byte)block;
            
            var s = _slots[slot];
            s.PendingBlock = block;
            s.LastQueryTime = DateTime.UtcNow;
            lock (_writeQueue) 
                _writeQueue.Enqueue(buffer);
        }
        
        internal void SendWrite(int slot, uint block, byte[] data)
        {
            var buffer = new byte[32];
            buffer[0] = (byte)'W';
            buffer[1] = (byte)(0x10 + slot);
            buffer[2] = (byte)block;
            Buffer.BlockCopy(data, 0, buffer, 3, 0x10);
            lock (_writeQueue) 
                _writeQueue.Enqueue(buffer);
        }

        /// <summary>
        /// Sets the color of the portal lights.
        /// </summary>
        public void SetColor(byte r, byte g, byte b)
        {
            if (Device == null)
                return;
            var buffer = new byte[32];
            buffer[0] = (byte)'C';
            buffer[1] = r;
            buffer[2] = g;
            buffer[3] = b;
            lock (_writeQueue)
                _writeQueue.Enqueue(buffer);
        }
        
        /// <summary>
        /// Sets the color of the portal lights.
        /// </summary>
        public void SetColor(Color color)
        {
            if (Device == null)
                return;
            var buffer = new byte[32];
            buffer[0] = (byte)'C';
            buffer[1] = color.R;
            buffer[2] = color.G;
            buffer[3] = color.B;
            lock (_writeQueue)
                _writeQueue.Enqueue(buffer);
        }

        private void Work(CancellationToken token)
        {
            var readBuffer = new byte[32];

            while (!token.IsCancellationRequested)
            {
                byte[] sendData = null;
                lock (_writeQueue)
                {
                    if (_writeQueue.Any())
                        sendData = _writeQueue.Dequeue();
                }

                if (sendData != null)
                {
                    try
                    {
                        var packet = new UsbSetupPacket(
                            0x21,
                            0x09,
                            0x0200, 
                            0x0000, 
                            sendData.Length
                        );
                        Device.ControlTransfer(packet, sendData, 0, sendData.Length);
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(new PortalWriteException($"Failed to write to portal: {ex.Message}"));
                    }
                }
                
                try
                {
                    int bytesRead;
                    var err = _reader.Transfer(readBuffer, 0, 0x20, 0, out bytesRead);
                    if (err == Error.Success && bytesRead > 0)
                    {
                        var data = new byte[bytesRead];
                        Array.Copy(readBuffer, data, bytesRead);
                        ParseResponse(data);
                    }
                    if (err == Error.Io)
                        Dispose();
                }
                catch (TimeoutException)
                {
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(new PortalReadException($"Failed to read from portal: {ex.Message}"));
                    break;
                }
                
                foreach (var slot in _slots)
                {
                    if (slot.PendingBlock == uint.MaxValue)
                        continue;

                    if ((DateTime.UtcNow - slot.LastQueryTime).TotalMilliseconds > 200)
                    {
                        if (slot.RetryCount > 5)
                        {
                            slot.PendingBlock = uint.MaxValue;
                            continue;
                        }
                        SendQuery(slot.Index, slot.PendingBlock);
                        slot.RetryCount++;
                    }
                }
                
                if (sendData == null)
                    Thread.Sleep(10);
            }
        }
        
        internal void SkylanderPlaced(int slotIndex, Skylander skylander)
        {
            try
            {
                OnSkylanderPlaced?.Invoke(slotIndex, skylander);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
        }
        
        internal void SkylanderProcessed(int slotIndex, Skylander skylander)
        {
            try
            {
                OnSkylanderProcessed?.Invoke(slotIndex, skylander);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
        }
        
        internal void SkylanderSaved(int slotIndex, Skylander skylander)
        {
            try
            {
                OnSkylanderSaved?.Invoke(slotIndex, skylander);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
        }

        /// <summary>
        /// Close the portal and clean up resources.
        /// </summary>
        public void Dispose()
        {
            _cts?.Cancel();
            Device.Close();
            Device.Dispose();
            usbContext.Dispose();
            _cts?.Dispose();
            PortalFinder.Reset();
        }
    }
}