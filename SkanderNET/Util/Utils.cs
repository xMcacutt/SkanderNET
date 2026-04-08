using System.Runtime.InteropServices;

namespace SkanderNET.Util
{
    internal static class Utils
    {
        internal static uint ReadBits(byte[] data, int bitOffset, int bitCount)
        {
            var value = 0;
            for (var i = 0; i < bitCount; i++)
            {
                var byteIndex = (bitOffset + i) / 8;
                var bitIndex = (bitOffset + i) % 8;
                var bit = (data[byteIndex] >> bitIndex) & 1;
                value |= (bit << i);
            }
            return (uint)value;
        }

        internal static void WriteBits(byte[] data, int bitOffset, int bitCount, uint value)
        {
            for (var i = 0; i < bitCount; i++)
            {
                var byteIndex = (bitOffset + i) / 8;
                var bitIndex = (bitOffset + i) % 8;
                var bit = (value >> i) & 1;
                if (bit == 1)
                    data[byteIndex] |= (byte)(1 << bitIndex);
                else
                    data[byteIndex] &= (byte)~(1 << bitIndex);
            }
        }
        
        internal static T ByteArrayToStruct<T>(byte[] data) where T : struct
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

        internal static byte[] StructToByteArray<T>(T obj) where T : struct
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