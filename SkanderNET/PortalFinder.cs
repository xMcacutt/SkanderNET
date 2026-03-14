using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;

namespace SkanderNET
{
    public class PortalFinder
    {
        private static readonly int Vid = 0x1430;
        private static readonly int[] Pids = { 0x1F17, 0x0150 };
        public static Action<Portal> OnPortalFound;
        private static bool _activePortal;

        internal static void Reset() { _activePortal = false; }
        
        public static async Task FindPortals(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                foreach (var pid in Pids)
                {
                    if (_activePortal)
                        break;
                    var context = new UsbContext();
                    var devices = context.List();
                    var device = devices.FirstOrDefault(d => d.VendorId == Vid && d.ProductId == pid);
                    if (device != null)
                    {
                        var portal = new Portal(device, context);
                        OnPortalFound.Invoke(portal);
                        _activePortal = true;
                    }

                    foreach (var usbDevice in devices.Where(d => d != device))
                        usbDevice.Dispose();
                    if (device == null)
                        context.Dispose();
                }

                await Task.Delay(1000, ct);
            }
        }
    }
}