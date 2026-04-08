using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SkanderNET.Crypto;
using SkanderNET.Data;
using SkanderNET.Data.Cyos;
using SkanderNET.Exceptions;
using SkanderNET.PortalComms;
using SkanderNET.Util;

namespace SkanderNET.Figures
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
        
        /// <summary>
        /// Marks the figure for formatting.
        /// The figure will then be formatted the next time it is placed on a portal or loaded with this library
        /// </summary>
        public void Reset()
        {
            Session.MarkForFormat();
        }

        internal static void Format(byte[] rawFigure)
        {
            var block = new byte[0x30];
            Buffer.BlockCopy(block, 0, rawFigure, 0x80 + 0x9C, 0x14);
            Buffer.BlockCopy(block, 0, rawFigure, 0x80 + 0xC0, 0x30);
            Buffer.BlockCopy(block, 0, rawFigure, 0x80 + 0x100, 0x1);
            rawFigure[0x89] = 0x1;
            rawFigure[0x8E] = 0x05;
            rawFigure[0x8F] = 0x00;
            var crc1Area = new byte[0x100];
            Buffer.BlockCopy(rawFigure, 0x80 + 0x50, crc1Area, 0x0, 0x20);
            Buffer.BlockCopy(rawFigure, 0x80 + 0x80, crc1Area, 0x0 + 0x20, 0x30);
            Buffer.BlockCopy(rawFigure, 0x80 + 0xC0, crc1Area, 0x0 + 0x50, 0x30);
            Buffer.BlockCopy(rawFigure, 0x80 + 0x100, crc1Area, 0x0 + 0x80, 0x30);
            Buffer.BlockCopy(rawFigure, 0x80 + 0x140, crc1Area, 0x0 + 0xB0, 0x30);
            Buffer.BlockCopy(rawFigure, 0x80 + 0x180, crc1Area, 0x0 + 0xE0, 0x20);
            var crc2Area = new byte[0x30];
            Buffer.BlockCopy(rawFigure, 0x80 + 0x10, crc2Area, 0x0, 0x20);
            Buffer.BlockCopy(rawFigure, 0x80 + 0x40, crc2Area, 0x20, 0x10);
            var crc1 = CRC16_IBM3740.Generate(crc1Area);
            var crc2 = CRC16_IBM3740.Generate(crc2Area);
            Buffer.BlockCopy(BitConverter.GetBytes(crc1), 0, rawFigure, 0x8A, 0x2);
            Buffer.BlockCopy(BitConverter.GetBytes(crc2), 0, rawFigure, 0x8C, 0x2);
            var headerBlock = new byte[0x10];
            Buffer.BlockCopy(rawFigure, 0x80, headerBlock, 0x0, 0x10);
            var crc3 = CRC16_IBM3740.Generate(headerBlock);
            Buffer.BlockCopy(BitConverter.GetBytes(crc3), 0, rawFigure, 0x8E, 0x2);
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
        
        /// <summary>
        /// Saves changes made to the figure back to the figure or file
        /// </summary>
        /// <exception cref="ChecksumGenerationFailureException">Raised when checksum generation fails</exception>
        public void Save()
        {
            _data.AreaSequenceValue++;
            SyncData();
            if (!GenerateChecksums())
                throw new ChecksumGenerationFailureException("Saving Creation Crystal failed, checksum generation was unsuccessful.");
            Session.SaveFigure(this, RawData);
        }

        /// <summary>
        /// Amount of gold collected by the character
        /// </summary>
        public ushort Money
        {
            get { return _data.Money; }
            set { _data.Money = value; }
        }

        /// <summary>
        /// Amount of time spent playing as the character in seconds
        /// </summary>
        public uint PlayTime => _data.TotalPlayTime;

        /// <summary>
        /// Which consoles/platforms the figure has been loaded on
        /// </summary>
        public Platform Platforms
        {
            get { return _data.Platforms; }
            set { _data.Platforms = value; }
        }
        
        /// <summary>
        /// The in-game nickname of the character set by the player
        /// </summary>
        public string Nickname
        {
            get { return _data.Nickname; }
            set { _data.Nickname = value; }
        }
        
        /// <summary>
        /// The date and time of the last reset to the figure
        /// </summary>
        /// <remarks>
        /// Seconds are always zero
        /// </remarks>
        public DateTime LastResetTime => _data.LastReset;
        
        /// <summary>
        /// The date and time that the figure was last saved
        /// </summary>
        /// <remarks>
        /// Seconds are always zero
        /// </remarks>
        public DateTime LastPlacedTime => _data.LastPlaced;

        /// <summary>
        /// Number of times that ownership of the figure has been transferred
        /// </summary>
        public byte OwnershipChangedCount => _data.OwnershipChangedCount;

        /// <summary>
        /// The battleclass selected for the imaginator
        /// </summary>
        public CyosBattleClass BattleClass
        {
            get
            {
                return _data.CyosData.BattleClass;
            }
            set
            {
                var data = _data.CyosData;
                data.BattleClass = value;
                _data.CyosData = data;
            }
        } 

        /// <summary>
        /// A quick shorthand property for grabbing the imaginator's selected customizations in a single string
        /// </summary>
        public string CyosInfo => _data.CyosData.ToString();
        
        /// <summary>
        /// The tasset selected for the imaginator
        /// </summary>
        /// <remarks>
        /// This is only applicable when the battleclass is Swashbuckler and otherwise returns CyosTasset.None
        /// </remarks>
        public CyosTasset Tasset
        {
            get {
                return _data.CyosData.Tasset;
            }
            set 
            {
                var data = _data.CyosData;
                data.Tasset = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The legs selected for the imaginator
        /// </summary>
        /// <remarks>
        /// This is only applicable when the battleclass is not Swashbuckler and otherwise returns CyosLegs.None
        /// </remarks>
        public CyosLegs Legs 
        {
            get {
                return _data.CyosData.Legs;
            }
            set 
            {
                var data = _data.CyosData;
                data.Legs = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The tail selected for the imaginator
        /// </summary>
        /// <remarks>
        /// This is only applicable when the battleclass is not Swashbuckler and otherwise returns CyosTail.None
        /// </remarks>
        public CyosTail Tail
        {
            get {
                return _data.CyosData.Tail;
            }
            set 
            {
                var data = _data.CyosData;
                data.Tail = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The ears selected for the imaginator
        /// </summary>
        public CyosEars Ears 
        {
            get {
                return _data.CyosData.Ears;
            }
            set 
            {
                var data = _data.CyosData;
                data.Ears = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The arms selected for the imaginator
        /// </summary>
        public CyosArms Arms 
        {
            get {
                return _data.CyosData.Arms;
            }
            set 
            {
                var data = _data.CyosData;
                data.Arms = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The aura selected for the imaginator
        /// </summary>
        public CyosAura Aura
        {
            get {
                return _data.CyosData.Aura;
            }
            set 
            {
                var data = _data.CyosData;
                data.Aura = value;
                _data.CyosData = data;
            }
        } 
            
        /// <summary>
        /// The headgear selected for the imaginator
        /// </summary>
        public CyosHeadgear Headgear 
        {
            get {
                return _data.CyosData.Headgear;
            }
            set 
            {
                var data = _data.CyosData;
                data.Headgear = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The arm guards selected for the imaginator
        /// </summary>
        public CyosArmGuards ArmGuards 
        {
            get {
                return _data.CyosData.ArmGuards;
            }
            set 
            {
                var data = _data.CyosData;
                data.ArmGuards = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The leg guards selected for the imaginator
        /// </summary>
        public CyosLegGuards LegGuards 
        {
            get {
                return _data.CyosData.LegGuards;
            }
            set 
            {
                var data = _data.CyosData;
                data.LegGuards = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The shoulder guards selected for the imaginator
        /// </summary>
        public CyosShoulderGuards ShoulderGuards 
        {
            get {
                return _data.CyosData.ShoulderGuards;
            }
            set 
            {
                var data = _data.CyosData;
                data.ShoulderGuards = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The backpack selected for the imaginator
        /// </summary>
        public CyosBackpack Backpack
        {
            get { return _data.CyosData.Backpack; }
            set 
            {
                var data = _data.CyosData;
                data.Backpack = value;
                _data.CyosData = data; 
            }
        }

        /// <summary>
        /// The voice selected for the imaginator
        /// </summary>
        public CyosVoice Voice 
        {
            get 
            {
                return _data.CyosData.Voice;
            }
            set 
            {
                var data = _data.CyosData;
                data.Voice = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The voice filter selected for the imaginator
        /// </summary>
        public CyosVoiceFilter VoiceFilter 
        {
            get 
            {
                return _data.CyosData.VoiceFilter;
            }
            set 
            {
                var data = _data.CyosData;
                data.VoiceFilter = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The music selected for the imaginator
        /// </summary>
        public CyosMusic Music 
        {
            get 
            {
                return _data.CyosData.Music;
            }
            set 
            {
                var data = _data.CyosData;
                data.Music = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The sound effects selected for the imaginator
        /// </summary>
        public CyosSoundEffects SoundEffects 
        {
            get 
            {
                return _data.CyosData.SoundEffects;
            }
            set 
            {
                var data = _data.CyosData;
                data.SoundEffects = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The first half of the catchphrase selected for the imaginator
        /// </summary>
        /// <remarks>
        /// This is the index of the catchphrase in the list of all catchphrases (not currently documented)
        /// </remarks>
        public uint Catchphrase1 
        {
            get 
            {
                return _data.CyosData.Catchphrase1;
            }
            set 
            {
                var data = _data.CyosData;
                data.Catchphrase1 = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The second half of the catchphrase selected for the imaginator
        /// </summary>
        /// <remarks>
        /// This is the index of the catchphrase in the list of all catchphrases (not currently documented)
        /// </remarks>
        public uint Catchphrase2 
        {
            get 
            {
                return _data.CyosData.Catchphrase2;
            }
            set 
            {
                var data = _data.CyosData;
                data.Catchphrase2 = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The height of the imaginator
        /// </summary>
        /// <remarks>
        /// An integer between 1 and 8 inclusive
        /// </remarks>
        public uint Height 
        {
            get 
            {
                return _data.CyosData.Height;
            }
            set 
            {
                var data = _data.CyosData;
                data.Height = value;
                _data.CyosData = data;
            }
        }
        
        /// <summary>
        /// The scale of the upper body of the imaginator
        /// </summary>
        /// <remarks>
        /// An integer between 1 and 8 inclusive
        /// </remarks>
        public uint UpperBodyScale 
        {
            get 
            {
                return _data.CyosData.UpperBodyScale;
            }
            set 
            {
                var data = _data.CyosData;
                data.UpperBodyScale = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The scale of the lower body of the imaginator
        /// </summary>
        /// <remarks>
        /// An integer between 1 and 8 inclusive
        /// </remarks>
        public uint LowerBodyScale 
        {
            get 
            {
                return _data.CyosData.LowerBodyScale;
            }
            set 
            {
                var data = _data.CyosData;
                data.LowerBodyScale = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The scale of the muscles of the imaginator
        /// </summary>
        /// <remarks>
        /// An integer between 1 and 8 inclusive
        /// </remarks>
        public uint MuscleScale 
        {
            get 
            {
                return _data.CyosData.MuscleScale;
            }
            set 
            {
                var data = _data.CyosData;
                data.MuscleScale = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The scale of the head of the imaginator
        /// </summary>
        /// <remarks>
        /// An integer between 1 and 8 inclusive
        /// </remarks>
        public uint HeadScale 
        {
            get 
            {
                return _data.CyosData.HeadScale;
            }
            set 
            {
                var data = _data.CyosData;
                data.HeadScale = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The width of the tail of the imaginator
        /// </summary>
        /// <remarks>
        /// An integer between 1 and 8 inclusive
        /// </remarks>
        public uint TailWidth 
        {
            get 
            {
                return _data.CyosData.TailWidth;
            }
            set 
            {
                var data = _data.CyosData;
                data.TailWidth = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The weapon selected for the imaginator
        /// </summary>
        /// <remarks>
        /// For the weapon to display, it must match the battleclass of the imaginator
        /// </remarks>
        public CyosWeapon Weapon
        {
            get
            {
                return _data.CyosData.PrimaryWeapon;
            }
            set
            {
                var data = _data.CyosData;
                data.PrimaryWeapon = value;
                _data.CyosData = data;
            }
        }
        
        /// <summary>
        /// The primary power (weapon power) selected for the imaginator
        /// </summary>
        public CyosWeaponPower WeaponPower 
        {
            get 
            {
                return _data.CyosData.WeaponPower;
            }
            set 
            {
                var data = _data.CyosData;
                data.WeaponPower = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The secondary power (elemental power) selected for the imaginator
        /// </summary>
        public CyosElementalPower ElementalPower 
        {
            get 
            {
                return _data.CyosData.ElementalPower;
            }
            set 
            {
                var data = _data.CyosData;
                data.ElementalPower = value;
                _data.CyosData = data;
            }
        }
            
        /// <summary>
        /// The tertiary power (secret technique) selected for the imaginator
        /// </summary>
        public CyosSecretTechnique SecretTechnique 
        {
            get 
            {
                return _data.CyosData.SecretTechnique;
            }
            set 
            {
                var data = _data.CyosData;
                data.SecretTechnique = value;
                _data.CyosData = data;
            }
        }
            
    }
}