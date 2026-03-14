using System;
using System.Runtime.InteropServices;

namespace SkanderNET
{
    public static class Utils
    {
        public static T ByteArrayToStruct<T>(byte[] data) where T : struct
        {
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(
                    handle.AddrOfPinnedObject(),
                    typeof(T)
                );
            }
            finally
            {
                handle.Free();
            }
        }

        public static byte[] StructToByteArray<T>(T obj) where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            var arr = new byte[size];

            var ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(obj, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            return arr;
        }
    }
}