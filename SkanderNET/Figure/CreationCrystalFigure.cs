using System;
using System.Runtime.InteropServices;

namespace SkanderNET
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct CreationCrystal
    {
        private fixed byte _padding0x0[3];
        internal ushort Money;
        internal uint PlayTime;
        internal byte AreaSequenceValue;
        internal ushort Crc16Type3;
        internal ushort Crc16Type2;
        internal ushort Crc16Type1;
        private fixed byte _padding0x10[3];
        internal Platform2011 Platform2011;
        private fixed byte _padding0x14[3];
        internal Platform2013 Platform2013;
        private byte _padding0x18;
        internal Element FigureElement;
        private fixed byte _padding0x1A[0x6];
        internal fixed byte NicknamePart1[0x10];
        internal fixed byte NicknamePart2[0x10];
        private byte _lastPlacedMinute;
        private byte _lastPlacedHour;
        private byte _lastPlacedDay;
        private byte _lastPlacedMonth;
        private ushort _lastPlacedYear;
        private fixed byte _padding0x46[0x9];
        internal byte OwnershipChangedCount;
        private byte _lastResetMinute;
        private byte _lastResetHour;
        private byte _lastResetDay;
        private byte _lastResetMonth;
        private ushort _lastResetYear;
        private fixed byte _padding0x56[0xA];
        private fixed byte _usageInfo[0xF];
        private fixed byte _padding0x6f[0xD];
        private fixed byte _cyosData[0x45];

        internal CyosData CyosData
        {
            get
            {
                fixed (byte* cyosDataBytePtr = _cyosData)
                {
                    byte[] cyosData = new byte[0x45];
                    Marshal.Copy((IntPtr)cyosDataBytePtr, cyosData, 0, cyosData.Length);
                    return new CyosData(cyosData);
                }
            }
            set
            {
                var data = value.GetData();
                if (data.Length != 0x45)
                    return;
                fixed (byte* cyosDataBytePtr = _cyosData)
                {
                    Marshal.Copy(data, 0, (IntPtr)cyosDataBytePtr, data.Length);
                }
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
    }
    
    internal class CreationCrystalFigure : Figure
    {
        internal CreationCrystalFigure(FigureSession session, TagHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }
}