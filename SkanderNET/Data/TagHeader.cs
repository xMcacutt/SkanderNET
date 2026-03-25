using System.Runtime.InteropServices;

namespace SkanderNET
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TagHeader
    {
        internal uint SerialNumber;
        private uint unk0x4;
        private uint unk0x8;
        private uint unk0xC;
        internal ushort ToyTypeId;
        private byte unk0x12;
        private byte ErrorByte;
        internal ulong TradingCardId;
        internal ushort VariantId;
        private ushort crc16Type0;
    }
}