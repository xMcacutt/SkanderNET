using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SkanderNET
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Villain
    {
        public VillainType VillainType;
        private byte _isEvolved;
        public byte _hat;
        public TrinketType Trinket;
        private fixed byte _nickname[0x20];
        private fixed byte padding0xC4[0xC];
        
        public bool IsEvolved
        {
            get { return _isEvolved != 0x0; }
            set { _isEvolved = value ? (byte)1 : (byte)0; }
        }
        
        public Hat Hat
        {
            get { return (Hat)_hat; }
            set { _hat = (byte)value; }
        }
        
        public string Nickname
        {
            get
            {
                var bytes = new byte[0x20];
                fixed (byte* ptr = _nickname)
                    Marshal.Copy((IntPtr)ptr, bytes, 0, 0x20);
                var str = Encoding.Unicode.GetString(bytes, 0, 32).Trim();
                if (str.Any(c => !char.IsLetterOrDigit(c) && !char.IsPunctuation(c) && !char.IsWhiteSpace(c) && c != 0x0) || str.All(c => char.IsWhiteSpace(c) || c == 0x0))
                    return null;
                var nullIndex = str.IndexOf('\0');
                if (nullIndex >= 0)
                    str = str.Substring(0, nullIndex);
                return str;
            }
            set
            {
                var bytes = new byte[0x20];
                var encoded = Encoding.Unicode.GetBytes(value ?? "");
                var length = Math.Min(encoded.Length, 0x20);
                Array.Copy(encoded, bytes, length);
                fixed (byte* ptr = _nickname)
                {
                    for (var i = 0; i < 0x20; i++)
                        ptr[i] = bytes[i];
                }
            }
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct Trap
    {
        private byte _containsPreTrappedVillain;
        internal byte NumberOfVillainsStored;
        private fixed byte padding0x2[0x5];
        private ushort _figureVillainType;
        internal byte AreaSequenceValue;
        internal ushort Crc1;
        internal ushort Crc2;
        internal ushort Crc3;
        internal Villain PrimaryVillain;
        internal Villain CachedVillain1;
        internal Villain CachedVillain2;
        internal Villain CachedVillain3;
        internal Villain CachedVillain4;
        internal Villain CachedVillain5;
        private fixed byte _usageInfo[0x10];

        internal bool ContainsPreTrappedVillain => _containsPreTrappedVillain == 1;
        internal VillainType PreTrappedVillainType => (VillainType)_figureVillainType;
    }
    
    public class TrapFigure : Figure
    {
        private Trap _data;
        private Portal _portal;
        
        internal TrapFigure(FigureSession session, ToyHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
            _portal = session.Portal;
            _data = Utils.ByteArrayToStruct<Trap>(RawData);
        }
        
        private void SyncData()
        {
            var bytes = Utils.StructToByteArray(_data);
            Buffer.BlockCopy(bytes, 0, RawData, 0, bytes.Length);
        }
        
        private bool GenerateChecksums()
        {
            var crc1Area = RawData.Skip(0x40).Take(0x110).ToArray();
            var crc2Area = RawData.Skip(0x10).Take(0x30).ToArray();
            _data.Crc1 = CRC16_IBM3740.Generate(crc1Area);
            _data.Crc2 = CRC16_IBM3740.Generate(crc2Area);
            Buffer.BlockCopy(BitConverter.GetBytes(_data.Crc1), 0, RawData, 0xA, 0x2);
            Buffer.BlockCopy(BitConverter.GetBytes(_data.Crc2), 0, RawData, 0xC, 0x2);
            var headerBlock = new byte[0x10];
            Buffer.BlockCopy(RawData, 0x0, headerBlock, 0x0, 0x10);
            headerBlock[0xE] = 0x05;
            headerBlock[0xF] = 0x00;
            _data.Crc3 = CRC16_IBM3740.Generate(headerBlock);
            Buffer.BlockCopy(BitConverter.GetBytes(_data.Crc3), 0, RawData, 0xE, 0x2);
            return true;
        }
        
        public void Reset()
        {
            if (_data.ContainsPreTrappedVillain)
                return;
            Session.MarkForFormat();
        }

        internal static void Format(byte[] rawFigure)
        {
            var block = new byte[0x30];
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x80 + 0x00, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x80 + 0x40, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x80 + 0x80, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x80 + 0xC0, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x80 + 0x100, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x80 + 0x140, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x80 + 0x180, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x240 + 0x00, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x240 + 0x40, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x240 + 0x80, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x240 + 0xC0, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x240 + 0x100, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x240 + 0x140, 0x30);
            Buffer.BlockCopy(block, 0x0, rawFigure, 0x240 + 0x180, 0x30);
        }

        internal static bool Verify(List<DataArea> dataAreas)
        {
            for (var i = 0; i < dataAreas.Count; i++)
            {
                var dataArea = dataAreas[i];   
                var crc1Area = dataArea.Data.Skip(0x30).Take(0x110).ToArray();
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
            return dataAreas.Any(x => x.IsValid);
        }

        public void Save()
        {
            _data.AreaSequenceValue++;
            SyncData();
            if (!GenerateChecksums())
                throw new ChecksumGenerationFailureException("Saving Trap failed, checksum generation was unsuccessful.");
            Session.SaveFigure(this, RawData);
        }
        
        public int VillainsStored => _data.NumberOfVillainsStored;
        public bool ContainsPreTrappedVillain => _data.ContainsPreTrappedVillain;
        public VillainType PreTrappedVillainType => _data.PreTrappedVillainType;

        public Villain PrimaryVillain
        {
            get { return _data.PrimaryVillain; }
            set { _data.PrimaryVillain = value; }
        }

        public bool TryGetCachedVillain(int villainIndex, out Villain villain)
        {
            if (VillainsStored <= villainIndex || villainIndex > 4 || villainIndex < 0)
            {
                villain = new Villain();
                return false;
            }
            switch (villainIndex)
            {
                case 0: 
                    villain = _data.CachedVillain1;
                    return true;
                case 1: 
                    villain = _data.CachedVillain2;
                    return true;
                case 2: 
                    villain = _data.CachedVillain3;
                    return true;
                case 3: 
                    villain = _data.CachedVillain4;
                    return true;
                case 4: 
                    villain = _data.CachedVillain5;
                    return true;
                default: 
                    _portal.Error(new ArgumentOutOfRangeException(nameof(villainIndex)));
                    villain = new Villain();
                    return false;
            }
        }

        public bool TrySetCachedVillain(int villainIndex, Villain value)
        {
            if (villainIndex > 4 || villainIndex < 0)
                return false;
            switch (villainIndex)
            {
                case 0: 
                    _data.CachedVillain1 = value; 
                    break;
                case 1: 
                    _data.CachedVillain2 = value; 
                    break;
                case 2: 
                    _data.CachedVillain3 = value; 
                    break;
                case 3: 
                    _data.CachedVillain4 = value; 
                    break;
                case 4: 
                    _data.CachedVillain5 = value; 
                    break;
            }
            return true;
        }

        public void TrapVillain(VillainType villainType)
        {
            for (int i = 4; i > 0; i--)
            {
                Villain prevVillain;
                TryGetCachedVillain(i - 1, out prevVillain);
                TrySetCachedVillain(i, prevVillain);
            }
            TrySetCachedVillain(0, _data.PrimaryVillain);
            Villain newVillain = new Villain();
            newVillain.VillainType = villainType;
            _data.PrimaryVillain = newVillain;
            if (_data.NumberOfVillainsStored < 6)
                _data.NumberOfVillainsStored++;
        }
    }
}