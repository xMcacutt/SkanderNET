using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SkanderNET
{
    [Flags]
    public enum TrophyVillain : ushort
    {
        None = 0,
        Glumshanks = 1 << 0x0,
        DragonHunter = 1 << 0x1,
        MoneyBone = 1 << 0x2,
        ChompyMage = 1 << 0x3,
        DrKrankcase = 1 << 0x4,
        Mesmerelda = 1 << 0x5,
        CaptainFrightbeard = 1 << 0x6,
        GoldenQueen = 1 << 0x7,
        Spellslamzer = 1 << 0x8,
        TheGulper = 1 << 0x9,
        ChefPepperJack = 1 << 0xA,
        Stratosphere = 1 << 0xB,
        CapNCluck = 1 << 0xC,
        Wolfgang = 1 << 0xD,
        PainYatta = 1 << 0xE,
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RacePack
    {
        private fixed byte _padding0x0[0x9];
        internal byte AreaSequenceValue1;
        internal ushort Crc1;
        internal ushort Crc2;
        internal ushort Crc3;
        internal TrophyVillain CapturedVillains;
        private fixed byte _padding0x12[0x5E];
        internal ushort Crc4;
        internal byte AreaSequenceValue2;
    }
    
    public class RacePackFigure : Figure
    {
        private RacePack _data;
        private Portal _portal;
        
        internal RacePackFigure(FigureSession session, ToyHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
            _portal = session.Portal;
            _data = Utils.ByteArrayToStruct<RacePack>(RawData);
        }
        
        private void SyncData()
        {
            var bytes = Utils.StructToByteArray(_data);
            Buffer.BlockCopy(bytes, 0, RawData, 0, bytes.Length);
        }
        
        private bool GenerateChecksums()
        {
            var crc1Area = RawData.Skip(0x40).Take(0x30).Concat(new byte[0xE0]).ToArray();
            var crc2Area = RawData.Skip(0x10).Take(0x30).ToArray();
            _data.Crc1 = CRC16_IBM3740.Generate(crc1Area);
            _data.Crc2 = CRC16_IBM3740.Generate(crc2Area);
            Buffer.BlockCopy(BitConverter.GetBytes(_data.Crc1), 0, RawData, 0xA, 0x2);
            Buffer.BlockCopy(BitConverter.GetBytes(_data.Crc2), 0, RawData, 0xC, 0x2);
            var headerBlock = RawData.Take(0x10).ToArray();
            headerBlock[0xE] = 0x05;
            headerBlock[0xF] = 0x00;
            _data.Crc3 = CRC16_IBM3740.Generate(headerBlock);
            Buffer.BlockCopy(BitConverter.GetBytes(_data.Crc3), 0, RawData, 0xE, 0x2);
            var crc4Area = RawData.Skip(0x70).Take(0x40).ToArray();
            crc4Area[0] = 0x06;
            crc4Area[1] = 0x01;
            _data.Crc4 = CRC16_IBM3740.Generate(crc4Area);
            Buffer.BlockCopy(BitConverter.GetBytes(_data.Crc4), 0, RawData, 0x70, 0x2);
            return true;
        }
        
        public void Save()
        {
            _data.AreaSequenceValue1++;
            _data.AreaSequenceValue2++;
            SyncData();
            if (!GenerateChecksums())
                throw new ChecksumGenerationFailureException("Saving Racing Pack failed, checksum generation was unsuccessful.");
            Session.SaveFigure(this, RawData);
        }

        public void Reset()
        {
            Session.MarkForFormat();
        }
        
        internal static void Format(byte[] rawFigure)
        {
            var block = new byte[0x30];
            // Remove everything in raw figure except build diagnostics date
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x80, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0xC0, 0x10);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0xD0, 0xC);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0xE0, 0x10);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x100, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x140, 0x20);
            // Set area sequence values to 1
            rawFigure[0x89] = 0x1;
            rawFigure[0x112] = 0x1;
            // Set region count id to 1
            rawFigure[0x96] = 0x1;
            // Set crc placeholders
            rawFigure[0x8E] = 0x05;
            rawFigure[0x8F] = 0x00;
            rawFigure[0x110] = 0x06;
            rawFigure[0x111] = 0x01;
            // Set last reset time
            var now = DateTime.Now;
            var yearBytes = BitConverter.GetBytes((ushort)now.Year);
            Buffer.BlockCopy(yearBytes, 0, rawFigure, 0xE4, 2);
            rawFigure[0xE3] = (byte)now.Month;
            rawFigure[0xE2] = (byte)now.Day;
            rawFigure[0xE1] = (byte)now.Hour;
            rawFigure[0xE0] = (byte)now.Minute;
            // Calculate CRCs
            var crc1Area = new byte[0x30 + 0xE0];
            Buffer.BlockCopy(rawFigure, 0xD0, crc1Area, 0x0, 0x20);
            Buffer.BlockCopy(rawFigure, 0x100, crc1Area, 0x20, 0x10);
            var crc1 = CRC16_IBM3740.Generate(crc1Area);
            Buffer.BlockCopy(BitConverter.GetBytes(crc1), 0x0, rawFigure, 0x8A, 2);
            var precomputedCrc2 = BitConverter.GetBytes((ushort)0x194E);
            Buffer.BlockCopy(precomputedCrc2, 0x0, rawFigure, 0x8C, 2);
            var precomputedCrc4 = BitConverter.GetBytes((ushort)0x5B54);
            Buffer.BlockCopy(precomputedCrc4, 0x0, rawFigure, 0x110, 2);
            var crc3 = CRC16_IBM3740.Generate(rawFigure.Skip(0x80).Take(0x10).ToArray());
            Buffer.BlockCopy(BitConverter.GetBytes(crc3), 0, rawFigure, 0x8E, 2);
        }
        
        internal static bool Verify(List<DataArea> dataAreas, List<DataArea> extendedDataAreas)
        {
            for (var i = 0; i < dataAreas.Count; i++)
            {
                var dataArea = dataAreas[i];   
                var crc1Area = dataArea.Data.Skip(0x30).Take(0x30).Concat(new byte[0xE0]).ToArray();
                var crc2Area = dataArea.Data.Take(0x30).ToArray();
                var calculatedCrc1 = CRC16_IBM3740.Generate(crc1Area);
                var calculatedCrc2 = CRC16_IBM3740.Generate(crc2Area);
                var crc1 = BitConverter.ToUInt16(dataArea.Header, 0xA);
                var crc2 = BitConverter.ToUInt16(dataArea.Header, 0xC);
                var headerBlock = new byte[0x10];
                Buffer.BlockCopy(dataArea.Header, 0x0, headerBlock, 0x0, 0x10);
                headerBlock[0xE] = 0x05;
                headerBlock[0xF] = 0x00;
                var calculatedCrc3 = CRC16_IBM3740.Generate(headerBlock);
                var crc3 = BitConverter.ToUInt16(dataArea.Header, 0xE);
                if (calculatedCrc1 != crc1 || calculatedCrc2 != crc2 || calculatedCrc3 != crc3)
                    dataArea.IsValid = false;
                else
                    dataArea.IsValid = true;
            }

            for (var i = 0; i < extendedDataAreas.Count; i++)
            {
                var dataArea = extendedDataAreas[i];
                var crcArea = dataArea.Header.Concat(dataArea.Data).ToArray();
                crcArea[0x0] = 0x6;
                crcArea[0x1] = 0x1;
                var calculatedCrc = CRC16_IBM3740.Generate(crcArea);
                var crc = BitConverter.ToUInt16(dataArea.Header, 0x0);
                dataArea.IsValid = calculatedCrc == crc;
            }

            return dataAreas.Any(x => x.IsValid) && (!extendedDataAreas.Any() || extendedDataAreas.Any(x => x.IsValid));
        }

        public TrophyVillain CapturedVillains
        {
            get { return _data.CapturedVillains; }
            set { _data.CapturedVillains = value; }
        }
    }
}