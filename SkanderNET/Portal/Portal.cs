using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using static SkanderNET.LibUsb;

namespace SkanderNET
{
    public class Portal : IDisposable
    {
        internal IntPtr Device;
        private bool _disposed;
        private IntPtr _usbContext;
        
        private readonly Queue<byte[]> _writeQueue = new Queue<byte[]>();
        private readonly PortalSlot[] _slots = new PortalSlot[16];
        private bool _activated;
        private readonly Thread _workerThread;
        private volatile bool _cancelRequested;
        
        /// <summary>
        /// Invoked when a figure is initially placed and its first sector containing character info is processed.
        /// </summary>
        public event Action<int, Figure> OnFigurePlaced;
        /// <summary>
        /// Invoked when a figure has fully loaded.
        /// </summary>
        public event Action<int, Figure> OnFigureProcessed;
        /// <summary>
        /// Invoked when a figure is removed from the portal.
        /// </summary>
        public event Action<int, Figure> OnFigureRemoved;
        /// <summary>
        /// Invoked when <see cref="Figure.Save"/> is called on a <see cref="Figure"/>.
        /// </summary>
        public event Action<int, Figure> OnFigureSaved;
        /// <summary>
        /// Invoked when the portal is activated and ready to being receiving queries.
        /// </summary>
        public event Action OnReady;
        /// <summary>
        /// Invoked when the portal throws an exception.
        /// </summary>
        public event Action<Exception> OnError;

        private readonly PortalAudio _audio;

        internal Portal(IntPtr device, IntPtr context)
        {
            _usbContext = context; // Hold to prevent disposal before device disposal
            Device = device;
            libusb_set_configuration(Device, 1);
            libusb_claim_interface(Device, 0);
            _audio = new PortalAudio(this);
            for (var i = 0; i < 0x10; i++)
                _slots[i] = new PortalSlot { Index = i };
            _workerThread = new Thread(Work);
            _workerThread.IsBackground = true;
            _workerThread.Start();
        }
        
        /// <summary>
        /// Begins initial communication with the <see cref="Portal"/>
        /// </summary>
        public void Activate() => SendActivate();
        
        private void ParseResponse(byte[] data)
        {
            var responseId = (char)data[0];
            switch (responseId)
            {
                case 'S':
                    HandleStatusResponse(data);
                    break;
                case 'W':
                    HandleWriteResponse(data);
                    break;
                case 'R':
                    HandleResetResponse(data);
                    break;
                case 'A':
                    HandleActivateResponse(data);
                    break;
                case 'Q':
                    HandleQueryResponse(data);
                    break;
                case 'J':
                    HandleSyncStatusResponse(data);
                    break;
                case 'M':
                    _audio.HandleAudioResponse(data);
                    break;
                case 'L':
                    HandleSlideLightsResponse(data);
                    break;
            }
        }

        private void HandleWriteResponse(byte[] data) { }

        private void HandleSlideLightsResponse(byte[] data) { }

        private void HandleSyncStatusResponse(byte[] data) { }

        private void HandleResetResponse(byte[] data) => SendActivate();

        private void HandleActivateResponse(byte[] data)
        {
            _activated = true;
            SendSyncStatus();
            OnReady?.Invoke();
        }
        
        private void HandleQueryResponse(byte[] data)
        {
            var slotIndex = (uint)(data[1] - 0x10);
            uint blockIndex = data[2];
            if (slotIndex >= _slots.Length)
                return;
            var slot = _slots[slotIndex];
            if (slot.PendingBlock != blockIndex)
                return;
            slot.PendingBlock = uint.MaxValue;
            slot.RetryCount = 0;
            if (slot.CurrentFigureSession == null && blockIndex == 0)
                slot.CurrentFigureSession = new FigureSession(this, slot.Index);

            if (slot.CurrentFigureSession == null)
                return;

            var blockData = new byte[16];
            Array.Copy(data, 3, blockData, 0, 16);
            slot.CurrentFigureSession.HandleBlock(blockIndex, blockData);
        }

        private void HandleStatusResponse(byte[] data)
        {
            if (!_activated)
                return;
            var statusMask = BitConverter.ToUInt32(data, 1);
            foreach (var slot in _slots)
            {
                var newStatus = (int)(statusMask >> (slot.Index * 2)) & 3;
                var oldStatus = slot.Status;
                slot.Status = newStatus;
                if (newStatus != 0 && oldStatus == 0)
                    SendQuery(slot.Index, 0);
                else if (newStatus == 0 && oldStatus != 0)
                {
                    if (slot.CurrentFigureSession == null)
                        continue;
                    var handler = OnFigureRemoved;
                    SafeInvoke(handler, slot.Index, slot.CurrentFigure);
                    slot.CurrentFigure = null;
                    slot.CurrentFigureSession = null;
                    slot.Status = 0;
                }
            }
        }

        private void SendSlideLights(byte r, byte g, byte b)
        {
            if (Device == IntPtr.Zero)
                return;

            var buffer = new byte[32];
            buffer[0] = (byte)'L';

            buffer[1] = 0x00;
            buffer[2] = r;
            buffer[3] = g;
            buffer[4] = b;

            buffer[5] = r;
            buffer[6] = g;
            buffer[7] = b;

            lock (_writeQueue)
                _writeQueue.Enqueue(buffer);
            
            buffer[0] = (byte)'L';

            buffer[1] = 0x01;
            buffer[2] = r;
            buffer[3] = g;
            buffer[4] = b;

            buffer[5] = r;
            buffer[6] = g;
            buffer[7] = b;

            lock (_writeQueue)
                _writeQueue.Enqueue(buffer);
        }

        /// <summary>
        /// Plays an audio file from path.
        /// </summary>
        /// <remarks>Requires 16bit PCM WAV</remarks>
        /// <param name="filePath">Path to the file to be played.</param>
        /// <param name="volume">Optional volume float between 0 and 1.</param>
        public PortalSoundClip PlayAudio(string filePath, float volume = 1.0f) => _audio?.PlayAudio(filePath, Math.Max(0, Math.Min(1, volume)));
        
        /// <summary>
        /// Plays an audio file from raw data.
        /// </summary>
        /// <param name="pcmData">Data to be played.</param>
        /// <param name="sampleRate">Base sample rate of the input.</param>
        /// <param name="volume">Optional volume float between 0 and 1.</param>
        public PortalSoundClip PlayAudio(byte[] pcmData, int sampleRate, float volume = 1.0f) => _audio?.PlayAudio(pcmData, sampleRate, volume);

        /// <summary>
        /// Stops current audio playing.
        /// </summary>
        public void ClearAudio() => _audio.Clear();
        
        internal void SendAudioStart()
        {
            var buf = new byte[] { (byte)'M', 0x01 };
            lock (_writeQueue) _writeQueue.Enqueue(buf);
        }

        internal void SendAudioStop()
        {
            var buf = new byte[] { (byte)'M', 0x00 };
            lock (_writeQueue) _writeQueue.Enqueue(buf);
        }

        private void SendSyncStatus()
        {
            if (Device == IntPtr.Zero)
                return;
            var buffer = new byte[32];
            buffer[0] = (byte)'J';
            lock (_writeQueue)
                _writeQueue.Enqueue(buffer);
        }
        
        public void Sync() => SendSyncStatus();
        
        /// <summary>
        /// Resets the portal calling activate and resending a sync command.
        /// This also resets the lights of the portal.
        /// </summary>
        public void Reset() => SendReset();

        private void SendReset()
        {
            if (Device == IntPtr.Zero)
                return;
            var buffer = new byte[32];
            buffer[0] = (byte)'R';
            lock (_writeQueue)
                _writeQueue.Enqueue(buffer);
        }

        private void SendActivate()
        {
            if (Device == IntPtr.Zero)
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
            if (Device == IntPtr.Zero)
                return;
            var buffer = new byte[32];
            buffer[0] = (byte)'C';
            buffer[1] = r;
            buffer[2] = g;
            buffer[3] = b;
            lock (_writeQueue)
                _writeQueue.Enqueue(buffer);
            SendSlideLights(r, g, b);
        }
        
        /// <summary>
        /// Sets the color of the portal lights.
        /// </summary>
        public void SetColor(Color color) => SetColor(color.R, color.G, color.B);
        
        private void Work()
        {
            var readBuffer = new byte[32];

            while (!_cancelRequested)
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
                        libusb_control_transfer(
                            Device,
                            0x21,
                            0x09,
                            0x0200,
                            0x0000,
                            sendData,
                            (ushort)sendData.Length,
                            200
                        );
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(new PortalWriteException($"Failed to write to portal: {ex.Message}"));
                    }
                }

                try
                {
                    int bytesRead;
                    var result = libusb_interrupt_transfer(Device, 0x81, readBuffer, readBuffer.Length,
                        out bytesRead, 200);
                    if (result == 0 && bytesRead > 0)
                    {
                        var data = new byte[bytesRead];
                        Array.Copy(readBuffer, data, bytesRead);
                        ParseResponse(data);
                    }

                    if (result != 0 && result != -7)
                        Error(new UsbCommunicationsException($"USB error: {result}"));
                    if (result == -4 || result == -1)
                        break;
                }
                catch (TimeoutException)
                {
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(new PortalReadException($"Failed to read from portal: {ex.Message}\n{ex.StackTrace}"));
                    break;
                }
                
                foreach (var slot in _slots)
                {
                    if (slot.PendingBlock == uint.MaxValue)
                        continue;

                    if ((DateTime.UtcNow - slot.LastQueryTime).TotalMilliseconds > 300)
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
            
            CleanUp();
        }

        internal void SetFigure(int slotIndex, Figure figure)
        {
            _slots[slotIndex].CurrentFigure = figure;
        }
        
        internal void FigurePlaced(int slotIndex, Figure figure)
        {
            var handler = OnFigurePlaced;
            SafeInvoke(handler, slotIndex, figure);
        }
        
        internal void FigureProcessed(int slotIndex, Figure figure)
        {
            var handler = OnFigureProcessed;
            SafeInvoke(handler, slotIndex, figure);
        }
        
        internal void FigureSaved(int slotIndex, Figure figure)
        {
            var handler = OnFigureSaved;
            SafeInvoke(handler, slotIndex, figure);
        }

        internal void Error(Exception ex)
        {
            if (OnError == null) 
                return;
            foreach (Action<Exception> subscriber in OnError.GetInvocationList())
            {
                try
                {
                    subscriber(ex);
                }
                catch (Exception internalException)
                {
                    Console.WriteLine(internalException.Message);
                }
            }
        }

        private void SafeInvoke(Action<int, Figure> action, int slotIndex, Figure figure)
        {
            if (action == null) 
                return;
            foreach (Action<int, Figure> subscriber in action.GetInvocationList())
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        subscriber(slotIndex, figure);
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(ex);
                    }
                });
            }
        }

        /// <summary>
        /// Close the portal and clean up resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) 
                return;
            _disposed = true;
            _audio?.Close();
            _cancelRequested = true;
            if (_workerThread != null && _workerThread.IsAlive)
                _workerThread.Join();
        }

        private void CleanUp()
        {
            if (Device != IntPtr.Zero)
            {
                libusb_close(Device);
                Device = IntPtr.Zero;
            }
            PortalFinder.Reset();
        }
    }
}