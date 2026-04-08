using System;
using System.Linq;
using System.Threading;
using SkanderNET.Util;

namespace SkanderNET.PortalComms
{
    public static class PortalFinder
    {
        private static readonly ushort Vid = 0x1430;
        private static readonly ushort[] Pids = { 0x0150 };
        private static Action<Portal> _onPortalFound;
        private static Portal _currentPortal; 
        private static bool _activePortal;
        private static IntPtr _context;
        private static bool _cancelRequested;
        private static Thread _workerThread;
        private static int _searching;
        private static int _closing;
        
        /// <summary>
        /// Invoked when any error occurs during the portal finding search loop
        /// </summary>
        public static Action<Exception> OnError;
        
        /// <summary>
        /// Invoked when a portal is discovered.
        /// New subscribers will receive the current portal if already discovered prior to subscription.
        /// </summary>
        public static event Action<Portal> OnPortalFound
        {
            add
            {
                _onPortalFound += value;
                if (_currentPortal != null)
                    value(_currentPortal);
            }
            remove
            {
                _onPortalFound -= value;
            }
        }
        
        internal static void Reset() { _activePortal = false; }

        /// <summary>
        /// Starts the search loop and begins looking for connected portals
        /// </summary>
        public static void InitSearch()
        {
            if (Interlocked.Exchange(ref _searching, 1) == 1)
                return;
            _cancelRequested = false;
            _workerThread = new Thread(FindPortals);
            _workerThread.IsBackground = true;
            _workerThread.Start();
        }

        /// <summary>
        /// Stops the portal search loop cleans up resources
        /// </summary>
        public static void Close()
        {
            if (Interlocked.Exchange(ref _closing, 1) == 1)
                return;
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
                        if (handle == IntPtr.Zero) continue;
                        var portal = new Portal(handle, _context);
                        _currentPortal = portal;
                        _activePortal = true;
                        var handler = _onPortalFound;
                        handler?.Invoke(portal);
                        break;
                    }
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
        }
    }
}