using System;
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
        internal fixed byte ExperienceLevelBytes[3];
        private fixed byte _padding0x3[0x2];
        internal uint TotalPlayTime;
        private byte _areaSequenceValue1;
        internal ushort Crc16Type3;
        internal ushort Crc16Type2;
        internal ushort Crc16Type1;
        internal VehicleFlags VehicleFlags;
        private byte _padding0x12;
        internal Platform2011 Platform2011;
        internal byte RegionCountId; // TODO ALWAYS SET THIS TO 1 PLEASE FUTURE MATT
        internal Platform2013 Platform2013;
        internal VehicleDecorationType VehicleDecorationType;
        internal VehicleTopperType VehicleTopperType;
        internal VehicleNeonType VehicleNeonType;
        internal VehicleShoutType VehicleShoutType;
        private fixed byte _padding0x1C[0x12];
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
        internal ushort Crc16Type4;
        private byte _areaSequenceValue2;
        private fixed byte _padding0x63[0x5];
        internal ushort Gearbits;

        internal byte AreaSequenceValue
        {
            get
            {
                return _areaSequenceValue1;
            }
            set
            {
                _areaSequenceValue1 = value;
                _areaSequenceValue2 = value;
            }
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
    
    internal class VehicleFigure : Figure
    {
        internal VehicleFigure(FigureSession session, ToyHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
            
        }
    }
}