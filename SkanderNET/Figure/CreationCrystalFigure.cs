using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SkanderNET.Cyos;
using SkanderNET.Data.Cyos;

namespace SkanderNET
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct CreationCrystal
    {
        private fixed byte _padding0x0[3];
        internal ushort Money;
        internal uint TotalPlayTime;
        internal byte AreaSequenceValue;
        internal ushort Crc1;
        internal ushort Crc2;
        internal ushort Crc3;
        private fixed byte _padding0x10[3];
        private Platform2011 _platforms2011;
        private fixed byte _padding0x14[3];
        private Platform2013 _platforms2013;
        private byte _padding0x18;
        internal Element FigureElement;
        private fixed byte _padding0x1A[0x6];
        private fixed byte _nickname[0x20];
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
        
        internal string Nickname
        {
            get
            {
                var bytes = new byte[0x20];
                fixed (byte* ptr = _nickname)
                    Marshal.Copy((IntPtr)ptr, bytes, 0, 0x20);
                var str = Encoding.ASCII.GetString(bytes, 0, 0x20).Trim('\0');
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
                var encoded = Encoding.ASCII.GetBytes(value ?? "");
                var length = Math.Min(encoded.Length, 0x20);
                Array.Copy(encoded, bytes, length);
                fixed (byte* ptr = _nickname)
                {
                    for (var i = 0; i < 0x20; i++)
                        ptr[i] = bytes[i];
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
    
    public class CreationCrystalFigure : Figure
    {
        private CreationCrystal _data;
        private Portal _portal;
        
        internal CreationCrystalFigure(FigureSession session, ToyHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
            _portal = session.Portal;
            _data = Utils.ByteArrayToStruct<CreationCrystal>(RawData);
        }
        
        private void SyncData()
        {
            var bytes = Utils.StructToByteArray(_data);
            Buffer.BlockCopy(bytes, 0, RawData, 0, bytes.Length);
        }
        
        private bool GenerateChecksums()
        {
            var crc1Area = RawData.Skip(0x40).Take(0x100).ToArray();
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
                var crc1Area = dataArea.Data.Skip(0x30).Take(0x100).ToArray();
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
                throw new ChecksumGenerationFailureException("Saving Creation Crystal failed, checksum generation was unsuccessful.");
            Session.SaveFigure(this, RawData);
        }

        public ushort Money
        {
            get { return _data.Money; }
            set { _data.Money = value; }
        }

        public uint PlayTime => _data.TotalPlayTime;

        public Platform Platforms
        {
            get { return _data.Platforms; }
            set { _data.Platforms = value; }
        }
        
        public string Nickname
        {
            get { return _data.Nickname; }
            set { _data.Nickname = value; }
        }
        
        public DateTime LastResetTime => _data.LastReset;
        
        public DateTime LastPlacedTime => _data.LastPlaced;

        public byte OwnershipChangedCount => _data.OwnershipChangedCount;
        
        public CyosBattleClass BattleClass => _data.CyosData.BattleClass;

        public string CyosInfo => _data.CyosData.ToString();
        
        public CyosTasset Tasset =>  _data.CyosData.Tasset;

        public CyosLegs Legs => _data.CyosData.Legs;
        
        public CyosTail Tail =>  _data.CyosData.Tail;
        
        public CyosEars Ears => _data.CyosData.Ears;
        
        public CyosArms Arms => _data.CyosData.Arms;

        public CyosAura Aura => _data.CyosData.Aura;

        public CyosHeadgear Headgear => _data.CyosData.Headgear;
        
        public CyosArmGuards ArmGuards => _data.CyosData.ArmGuards;
        
        public CyosLegGuards LegGuards => _data.CyosData.LegGuards;

        public CyosShoulderGuards ShoulderGuards => _data.CyosData.ShoulderGuards;
        
        public CyosBackpack Backpack => _data.CyosData.Backpack;
        
        public CyosVoice Voice => _data.CyosData.Voice;
        
        public CyosVoiceFilter VoiceFilter => _data.CyosData.VoiceFilter;
        
        public CyosMusic Music => _data.CyosData.Music;

        public CyosSoundEffects SoundEffects => _data.CyosData.SoundEffects;
        
        public uint Catchphrase1 => _data.CyosData.Catchphrase1;
        
        public uint Catchphrase2 => _data.CyosData.Catchphrase2;
        
        public uint Height => _data.CyosData.Height;
        
        public uint UpperBodyScale => _data.CyosData.UpperBodyScale;
        
        public uint LowerBodyScale => _data.CyosData.LowerBodyScale;

        public uint MuscleScale => _data.CyosData.MuscleScale;

        public uint HeadScale => _data.CyosData.HeadScale;
        
        public uint TailWidth => _data.CyosData.TailWidth;

        public CyosWeapon Weapon => _data.CyosData.PrimaryWeapon;

        public CyosWeaponPower WeaponPower => _data.CyosData.WeaponPower;
        
        public CyosElementalPower ElementalPower => _data.CyosData.ElementalPower;
        
        public CyosSecretTechnique SecretTechnique => _data.CyosData.SecretTechnique;
    }
}