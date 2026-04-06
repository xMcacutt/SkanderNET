using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SkanderNET
{
    [Flags]
    internal enum VehicleFlags : ushort
    {
        None = 0,
        ShieldLevel1 = 1 << 0,
        ShieldLevel2 = 1 << 1,
        ShieldLevel3 = 1 << 2,
        ShieldLevel4 = 1 << 3,
        ShieldLevel5 = 1 << 4,
        WeaponLevel1 = 1 << 5,
        WeaponLevel2 = 1 << 6,
        WeaponLevel3 = 1 << 7,
        WeaponLevel4 = 1 << 8,
        WeaponLevel5 = 1 << 9,
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct Vehicle
    {
        // Block 0x8 - Has its own Area Sequence value
        private UInt24 _experienceSSCR;
        private fixed byte _padding0x3[0x2];
        internal uint TotalPlayTime;
        internal byte AreaSequenceValue1;
        internal ushort Crc1;
        internal ushort Crc2;
        internal ushort Crc3;
        internal VehicleFlags VehicleFlags;
        private byte _padding0x12;
        private Platform2011 _platforms2011;
        private fixed byte _padding0x14[0x2];
        internal byte RegionCountId;
        private Platform2013 _platforms2013;
        internal VehicleDecorationType VehicleDecorationType;
        internal VehicleTopperType VehicleTopperType;
        internal VehicleNeonType VehicleNeonType;
        internal VehicleShoutType VehicleShoutType;
        private fixed byte _padding0x1C[0x22];
        private ushort _vehicleMod;
        private byte _lastPlacedMinute;
        private byte _lastPlacedHour;
        private byte _lastPlacedDay;
        private byte _lastPlacedMonth;
        private ushort _lastPlacedYear;
        private fixed byte _padding0x36[0x6];
        private byte _lastBuildYear;
        private byte _lastBuildMonth;
        private byte _lastBuildDay;
        internal byte OwnershipChangedCount;
        private byte _lastResetMinute;
        private byte _lastResetHour;
        private byte _lastResetDay;
        private byte _lastResetMonth;
        private ushort _lastResetYear;
        private ushort _prEventData;
        private uint _wiiData;
        private uint _xboxData;
        private fixed byte _usageInfo[0xf];
        private byte _padding0x5f;
        // Block 0x11 - Has its own Area Sequence value
        internal ushort Crc4;
        internal byte AreaSequenceValue2;
        private fixed byte _padding0x63[0x5];
        internal ushort Gearbits;
        
        internal uint SuperchargersRacingExperience
        {
            get { return _experienceSSCR; }
            set { _experienceSSCR = value; }
        }
        
        internal DateTime LastReset
        {
            get
            {
                return new DateTime(_lastResetYear, _lastResetMonth, _lastResetDay, _lastResetHour, _lastResetMinute, 0);
            }
            set
            {
                _lastResetYear = (ushort)value.Year;
                _lastResetMonth = (byte)value.Month;
                _lastResetDay = (byte)value.Day;
                _lastResetHour = (byte)value.Hour;
                _lastResetMinute = (byte)value.Minute;
            }
        }
        
        internal DateTime LastBuild
        {
            get
            {
                return new DateTime(_lastBuildYear + 2000, _lastBuildMonth, _lastBuildDay);
            }
            set
            {
                _lastBuildYear = (byte)(value.Year - 2000);
                _lastBuildMonth = (byte)value.Month;
                _lastBuildDay = (byte)value.Day;
            }
        }

        internal DateTime LastPlaced
        {
            get
            {
                return new DateTime(_lastPlacedYear, _lastPlacedMonth, _lastPlacedDay, _lastPlacedHour, _lastPlacedMinute, 0);
            }
            set
            {
                _lastPlacedYear = (ushort)value.Year;
                _lastPlacedMonth = (byte)value.Month;
                _lastPlacedDay = (byte)value.Day;
                _lastPlacedHour = (byte)value.Hour;
                _lastPlacedMinute = (byte)value.Minute;
            }
        }
        
        internal Platform Platforms
        {
            get
            {
                var result = Platform.None;
                if ((_platforms2011 & Platform2011.Wii) != 0)
                    result |= Platform.Wii;
                if ((_platforms2011 & Platform2011.Xbox360) != 0)
                    result |= Platform.Xbox360;
                if ((_platforms2011 & Platform2011.PlayStation3) != 0)
                    result |= Platform.PlayStation3;
                if ((_platforms2011 & Platform2011.PC) != 0)
                    result |= Platform.PC;
                if ((_platforms2011 & Platform2011.Nintendo3ds) != 0)
                    result |= Platform.Nintendo3ds;
                if ((_platforms2013 & Platform2013.WiiU) != 0)
                    result |= Platform.WiiU;
                if ((_platforms2013 & Platform2013.XboxOne) != 0)
                    result |= Platform.XboxOne;
                if ((_platforms2013 & Platform2013.PlayStation4) != 0)
                    result |= Platform.PlayStation4;
                if ((_platforms2013 & Platform2013.iOS) != 0)
                    result |= Platform.iOS;
                if ((_platforms2013 & Platform2013.NintendoSwitch) != 0)
                    result |= Platform.NintendoSwitch;
                return result;
            }
            set
            {
                var p2011 = Platform2011.None;
                var p2013 = Platform2013.None;
                if ((value & Platform.Wii) != 0)
                    p2011 |= Platform2011.Wii;
                if ((value & Platform.Xbox360) != 0)
                    p2011 |= Platform2011.Xbox360;
                if ((value & Platform.PlayStation3) != 0)
                    p2011 |= Platform2011.PlayStation3;
                if ((value & Platform.PC) != 0)
                    p2011 |= Platform2011.PC;
                if ((value & Platform.Nintendo3ds) != 0)
                    p2011 |= Platform2011.Nintendo3ds;
                if ((value & Platform.WiiU) != 0)
                    p2013 |= Platform2013.WiiU;
                if ((value & Platform.XboxOne) != 0)
                    p2013 |= Platform2013.XboxOne;
                if ((value & Platform.PlayStation4) != 0)
                    p2013 |= Platform2013.PlayStation4;
                if ((value & Platform.iOS) != 0)
                    p2013 |= Platform2013.iOS;
                if ((value & Platform.NintendoSwitch) != 0)
                    p2013 |= Platform2013.NintendoSwitch;
                _platforms2011 = p2011;
                _platforms2013 = p2013;
            }
        }       
        
        internal int PerformanceMod
        {
            get { return _vehicleMod & 0xF; }
            set { _vehicleMod = (ushort)((_vehicleMod & ~0xF) | (value & 0xF)); }
        }
        
        internal int SpecialtyMod
        {
            get { return (_vehicleMod >> 4) & 0xF; }
            set { _vehicleMod = (ushort)((_vehicleMod & ~(0xF << 4)) | ((value & 0xF) << 4)); }
        }
        
        internal int HornMod
        {
            get { return (_vehicleMod >> 8) & 0xF; }
            set { _vehicleMod = (ushort)((_vehicleMod & ~(0xF << 8)) | ((value & 0xF) << 8)); }
        }
    }
    
    public class VehicleFigure : Figure
    {
        private Vehicle _data;
        private Portal _portal;
        private readonly VehicleTerrainType _terrain;
        
        internal VehicleFigure(FigureSession session, ToyHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
            _portal = session.Portal;
            _data = Utils.ByteArrayToStruct<Vehicle>(RawData);
            _terrain = metaData.VehicleTerrainType;
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
                throw new ChecksumGenerationFailureException("Saving vehicle failed, checksum generation was unsuccessful.");
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
        
        public uint SuperchargersRacingExperience
        {
            get { return _data.SuperchargersRacingExperience; }
            set { _data.SuperchargersRacingExperience = value; }
        }
        
        public ushort Gearbits
        {
            get { return _data.Gearbits; }
            set { _data.Gearbits = value; }
        }

        public uint PlayTime => _data.TotalPlayTime;
        public uint OwnershipChangedCount => _data.OwnershipChangedCount;

        public DateTime LastResetTime => _data.LastReset;
        
        public DateTime LastPlacedTime => _data.LastPlaced;
        
        public DateTime LastBuildTime => _data.LastBuild;

        public VehicleTerrainType TerrainType => _terrain;
        
        public VehicleDecorationType Decoration {
            get { return _data.VehicleDecorationType; }
            set { _data.VehicleDecorationType = value; }
        }
        
        public VehicleTopperType Topper {
            get { return _data.VehicleTopperType; }
            set { _data.VehicleTopperType = value; }
        }
        
        public VehicleNeonType Neon {
            get { return _data.VehicleNeonType; }
            set { _data.VehicleNeonType = value; }
        }
        
        public VehicleShoutType Shout {
            get { return _data.VehicleShoutType; }
            set { _data.VehicleShoutType = value; }
        }

        public VehicleMod GetVehicleMod(VehicleModType type)
        {
            var modIndex = type == VehicleModType.Performance 
                ? _data.PerformanceMod : type == VehicleModType.Specialty 
                    ? _data.SpecialtyMod : _data.HornMod;
            return VehicleMods.Mods[Toy][type][modIndex];
        }

        public void SetVehicleMod(VehicleModType type, int index)
        {
            switch (type)
            {
                case VehicleModType.Performance: _data.PerformanceMod = index; break;
                case VehicleModType.Specialty: _data.SpecialtyMod = index; break;
                case VehicleModType.Horn: _data.HornMod = index; break;
            }
        }
    }
}