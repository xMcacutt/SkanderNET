using System.Runtime.InteropServices;

namespace SkanderNET.Util
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct UInt24
    {
        private byte b0;
        private byte b1;
        private byte b2;

        private uint Value
        {
            get { return (uint)(b0 | (b1 << 8) | (b2 << 16)); } 
            set
            {
                b0 = (byte)(value & 0xFF);
                b1 = (byte)((value >> 8) & 0xFF);
                b2 = (byte)((value >> 16) & 0xFF);
            }
        }

        public static implicit operator uint(UInt24 v) => v.Value;
        public static implicit operator UInt24(uint v) => new UInt24 { Value = v };
    }
}