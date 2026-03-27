using System.Runtime.InteropServices;

namespace SkanderNET
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ToyHeader
    {
        private uint _serialNumber;
        private uint unk0x4;
        private uint unk0x8;
        private uint unk0xC;
        private ushort _toyTypeId;
        private byte unk0x12;
        private byte ErrorByte;
        private ulong _tradingCardId;
        private ushort _variantId;
        private ushort crc16Type0;
        
        public uint SerialNumber => _serialNumber;
        public ushort ToyTypeId => _toyTypeId;
        public ushort VariantId => _variantId;
        public ulong TradingCardId => _tradingCardId;
    }
}