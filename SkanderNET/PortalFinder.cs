using System;
using System.Linq;
using System.Threading;

namespace SkanderNET
{
    public class PortalFinder
    {
        private static readonly ushort Vid = 0x1430;
        private static readonly ushort[] Pids = { 0x1F17, 0x0150 };
        public static Action<Portal> OnPortalFound;
        private static bool _activePortal;
        private static IntPtr _context;
        private static bool _cancelRequested;
        private static Thread _workerThread;
        
        internal static void Reset() { _activePortal = false; }

        public static void InitSearch()
        {
            _workerThread = new Thread(FindPortals);
            _workerThread.IsBackground = true;
            _workerThread.Start();
        }

        public static void Close()
        {
            _cancelRequested = true;
            if (_workerThread != null && _workerThread.IsAlive)
                _workerThread.Join();
        } 
        
        private static void FindPortals()
        {
            try
            {

                LibUsb.libusb_init(out _context);

                while (!_cancelRequested)
                {
                    foreach (var pid in Pids)
                    {
                        if (_activePortal)
                            break;
                        var handle = LibUsb.libusb_open_device_with_vid_pid(_context, Vid, pid);
                        if (handle != IntPtr.Zero)
                        {
                            var portal = new Portal(handle, _context);
                            OnPortalFound?.Invoke(portal);
                            _activePortal = true;
                            break;
                        }
                    }

                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}