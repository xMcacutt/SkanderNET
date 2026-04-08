using System;
using System.Runtime.InteropServices;

namespace SkanderNET.Util
{
    internal static class LibUsb
    {
        private const string DllPath = "libusb-1.0.dll";

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int libusb_init(out IntPtr context);
        
        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void libusb_exit(IntPtr context);
        
        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr libusb_open_device_with_vid_pid(IntPtr context, ushort vendor_id, ushort product_id);
        
        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int libusb_claim_interface(IntPtr device, int interface_number);
        
        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int libusb_release_interface(IntPtr device, int interface_number);
        
        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int libusb_interrupt_transfer(IntPtr dev_handle, byte endpoint, byte[] data, int length, out int transferred, int timeout);
        
        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int libusb_control_transfer(IntPtr dev_handle, byte bmRequestType, byte bRequest, ushort wValue, ushort wIndex, byte[] data, ushort wLength, uint timeout);
        
        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void libusb_close(IntPtr dev_handle);
        
        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int libusb_set_configuration(IntPtr dev_handle, int configuration);
    }
}