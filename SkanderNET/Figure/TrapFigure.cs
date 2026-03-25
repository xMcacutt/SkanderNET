using System.Runtime.InteropServices;

namespace SkanderNET
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct Villain
    {
        internal VillainType PrimaryVillainType;
        internal byte IsEvolved;
        internal byte Hat;
        internal TrinketType Trinket;
        internal fixed byte NicknamePart1[0xC];
        internal fixed byte NicknamePart2[0x10];
        internal fixed byte NicknamePart3[0x4];
        internal fixed byte padding0xC4[0xC];
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct Trap
    {
        internal byte ContainsVillain;
        internal byte NumberOfVillainsStored;
        private fixed byte padding0x2[0x5];
        internal VillainType FigureVillainType;
        internal byte AreaSequenceValue;
        internal ushort Crc16Type3;
        internal ushort Crc16Type2;
        internal ushort Crc16Type1;
        internal Villain PrimaryVillain;
        internal Villain CachedVillain1;
        internal Villain CachedVillain2;
        internal Villain CachedVillain3;
        internal Villain CachedVillain4;
        internal Villain CachedVillain5;
        private fixed byte _usageInfo[0xf];
    }
    
    internal class TrapFigure : Figure
    {
        internal TrapFigure(FigureSession session, TagHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
            
        }

        private void Parse()
        {
            
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}