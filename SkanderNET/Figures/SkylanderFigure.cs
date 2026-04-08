using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using SkanderNET.Crypto;
using SkanderNET.Data;
using SkanderNET.Exceptions;
using SkanderNET.PortalComms;
using SkanderNET.Util;

namespace SkanderNET.Figures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct Skylander
    {
        private UInt24 _experience2011; // max 33000
        internal ushort Money;
        internal uint TotalPlayTime;
        internal byte AreaSequenceValue1;
        internal ushort Crc1;
        internal ushort Crc2;
        internal ushort Crc3;
        private UInt24 _flags1; 
        private Platform2011 _platforms2011;
        private ushort _hat2011;
        internal byte RegionCountId;
        private Platform2013 _platforms2013;
        internal ulong OwnerId;
        private fixed byte _nickname[0x20];
        private byte _lastPlacedMinute;
        private byte _lastPlacedHour;
        private byte _lastPlacedDay;
        private byte _lastPlacedMonth;
        private ushort _lastPlacedYear;
        internal HeroicChallengesSpyrosAdventure CompletedSpyrosAdventureHeroicChallenges;
        internal ushort HeroPoints;
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
        private fixed byte _usageInfo[0x10];
        // Block 0x11 - Has its own Area Sequence value
        internal ushort Crc4;
        internal byte AreaSequenceValue2; 
        private ushort _experience2012; // max 63500 in SF and 65535 in previous
        private byte _hat2012;
        private ushort _flags2;
        private uint _experience2013; // max 101000
        private byte _hat2013;
        internal TrinketType Trinket;
        private byte _hat2015; // Must add 0x100
        private byte _padding0x7f;
        private uint _battleGroundFlags;
        internal HeroicChallengesGiants CompletedGiantsHeroicChallenges;
        private byte _padding0x86;
        private fixed byte _giantsQuests[0x9];
        private fixed byte _unusedGiantsQuestsData[0x6];
        private fixed byte _swapForceQuests[0x9];
        
        internal string Nickname
        {
            get
            {
                var bytes = new byte[0x20];
                fixed (byte* ptr = _nickname)
                    Marshal.Copy((IntPtr)ptr, bytes, 0, 0x20);
                var str = Encoding.Unicode.GetString(bytes, 0, 30).Trim();
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
                var length = Math.Min(encoded.Length, 0x1F);
                Array.Copy(encoded, bytes, length);
                fixed (byte* ptr = _nickname)
                {
                    for (var i = 0; i < 0x20; i++)
                        ptr[i] = bytes[i];
                }
            }
        }

        internal Hat EquippedHatSSA
        {
            get
            {
                if (_hat2011 != 0)
                    return (Hat)_hat2011;
                return Hat.None;
            }
            set
            {
                var hatValue = (uint)value;
                if (hatValue > (int)Hat.VintageBaseballCap)
                    return;
                _hat2011 = 0;
                _hat2012 = 0;
                _hat2013 = 0;
                _hat2015 = 0;
                _hat2011 = (byte)hatValue;
            }
        }

        internal Hat EquippedHatGiants
        {
            get
            {
                if (_hat2011 != 0)
                    return (Hat)_hat2011;
                if (_hat2012 != 0)
                    return (Hat)_hat2012;
                if (_hat2013 != 0)
                    return (Hat)_hat2013;
                if (_hat2015 != 0 && RegionCountId == 1)
                    return (Hat)(_hat2015 + 0x100);
                return Hat.None;
            }
            set
            {
                var hatValue = (uint)value;
                if (hatValue > byte.MaxValue * 2)
                    return;
                _hat2011 = 0;
                _hat2012 = 0;
                _hat2013 = 0;
                _hat2015 = 0;
                if (hatValue <= (int)Hat.VintageBaseballCap)
                    _hat2011 = (byte)hatValue;
                else if (hatValue <= (int)Hat.GoldTopHat)
                    _hat2012 = (byte)hatValue;
                else if (hatValue <= byte.MaxValue)
                    _hat2013 = (byte)hatValue;
                else
                    _hat2015 = (byte)(hatValue - 0x100);
            }
        }

        internal Hat EquippedHatSwapForce
        {
            get
            {
                if (_hat2015 != 0 && RegionCountId == 1)
                    return (Hat)(_hat2015 + 0x100);
                if (_hat2013 != 0)
                    return (Hat)_hat2013;
                if (_hat2012 != 0)
                    return (Hat)_hat2012;
                if (_hat2011 != 0)
                    return (Hat)_hat2011;
                return Hat.None;
            }
            set
            {
                var hatValue = (uint)value;
                if (hatValue > byte.MaxValue * 2)
                    return;
                _hat2011 = 0;
                _hat2012 = 0;
                _hat2013 = 0;
                _hat2015 = 0;
                if (hatValue > byte.MaxValue)
                    _hat2015 = (byte)(hatValue - 0x100);
                if (hatValue > (int)Hat.GoldTopHat)
                    _hat2013 = (byte)hatValue;
                if (hatValue > (int)Hat.VintageBaseballCap)
                    _hat2012 = (byte)hatValue;
                else
                    _hat2011 = (byte)hatValue;
            }
        }
        
        internal uint Experience
        {
            get
            {
                return _experience2011 + (RegionCountId == 1 ? _experience2012 + _experience2013 : 0);
            }
            set
            {
                _experience2011 = Math.Min(value, 33000);
                var remaining = value - _experience2011;
                _experience2012 = (ushort)Math.Min(remaining, 63500);
                _experience2013 = remaining - _experience2012;
            }
        }
        
        internal UpgradeData UpgradeData
        {
            get
            {
                uint flags1 = _flags1;
                var flags2 = _flags2;
                var combined = ((uint)(flags2 & 0xF) << 10) | (flags1 & 0x3FFu);
                return new UpgradeData((ushort)combined);
            }
            set
            {
                uint flags1 = _flags1;
                flags1 = (flags1 & ~0x3FFu) | (value.Raw & 0x3FFu);
                _flags1 = flags1;

                var flags2 = _flags2;
                flags2 = (ushort)((flags2 & ~0xFu) | ((value.Raw >> 10) & 0xFu));
                _flags2 = flags2;
            }
        }
        
        internal byte SwapForceAccoladeRank
        {
            get
            {
                return (byte)((_flags2 >> 9) & 3);  
            } 
            set
            {
                var flags2 = _flags2;
                flags2 = (ushort)((flags2 & ~(3u << 9)) | ((value & 3u) << 9));
                _flags2 = flags2;
            }
        }

        internal byte GiantsAccoladeRank
        {
            get
            { 
                return (byte)((_flags2 >> 4) & 3);  
            } 
            set
            {
                var flags2 = _flags2;
                flags2 = (ushort)((flags2 & ~(3u << 4)) | ((value & 3u) << 4));
                _flags2 = flags2;
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

        internal BattleGroundFlags BattleGroundFlags
        {
            get
            {
                return new BattleGroundFlags(_battleGroundFlags);
            }
            set
            {
                _battleGroundFlags = value.Flags;
            }
        }
        
        internal SwapForceQuests SwapForceQuests
        {
            get
            {
                fixed (byte* swapForceQuestsBytePtr = _swapForceQuests)
                {
                    var swapForceQuestsData = new byte[0x9];
                    Marshal.Copy((IntPtr)swapForceQuestsBytePtr, swapForceQuestsData, 0, swapForceQuestsData.Length);
                    return new SwapForceQuests(swapForceQuestsData);
                }
            }
            set
            {
                var data = value.GetData();
                if (data.Length != 0x9)
                    return;
                fixed (byte* swapForceQuestsBytePtr = _swapForceQuests)
                {
                    Marshal.Copy(data, 0, (IntPtr)swapForceQuestsBytePtr, data.Length);
                }
            }
        }
        
        internal GiantsQuests GiantsQuests
        {
            get
            {
                fixed (byte* giantsQuestsBytePtr = _giantsQuests)
                {
                    var giantsQuestsData = new byte[0x9];
                    Marshal.Copy((IntPtr)giantsQuestsBytePtr, giantsQuestsData, 0, giantsQuestsData.Length);
                    return new GiantsQuests(giantsQuestsData);
                }
            }
            set
            {
                var data = value.GetData();
                if (data.Length != 0x9)
                    return;
                fixed (byte* giantsQuestsBytePtr = _giantsQuests)
                {
                    Marshal.Copy(data, 0, (IntPtr)giantsQuestsBytePtr, data.Length);
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
    }

    public class SkylanderFigure : Figure
    {
        private Skylander _data;
        private Portal _portal;
        
        
        internal SkylanderFigure(FigureSession session, ToyHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
            _portal = session.Portal;
            _data = Utils.ByteArrayToStruct<Skylander>(RawData);
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
        
        /// <summary>
        /// Saves changes made to the figure back to the figure or file
        /// </summary>
        /// <exception cref="ChecksumGenerationFailureException">Raised when checksum generation fails</exception>
        public void Save()
        {
            _data.AreaSequenceValue1++;
            _data.AreaSequenceValue2++;
            SyncData();
            if (!GenerateChecksums())
                throw new ChecksumGenerationFailureException("Saving skylander failed, checksum generation was unsuccessful.");
            Session.SaveFigure(this, RawData);
        }

        /// <summary>
        /// Marks the figure for formatting.
        /// The figure will then be formatted the next time it is placed on a portal or loaded with this library
        /// </summary>
        public void Reset()
        {
            Session.MarkForFormat();
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
        
        public uint Experience
        {
            get { return _data.Experience; }
            set { _data.Experience = value; }
        }
        
        public ushort Money
        {
            get { return _data.Money; }
            set { _data.Money = value; }
        }

        /// <summary>
        /// Total time played in seconds
        /// </summary>
        public uint PlayTime => _data.TotalPlayTime;

        public uint OwnershipChangedCount => _data.OwnershipChangedCount;
        
        public uint Level
        {
            get { return SkylanderExperience.GetLevel(Experience); }
            set { Experience = SkylanderExperience.GetExperience(value); }
        }

        /// <summary>
        /// Gets the hat worn by the skylander according to a specific game
        /// </summary>
        /// <param name="game">Which game's algorithm should be used for finding the first hat worn</param>
        /// <returns>The hat worn by the skylander</returns>
        public Hat GetHat(SkylandersGame game)
        {
            switch (game)
            {
                case SkylandersGame.SpyrosAdventure:
                    return _data.EquippedHatSSA;
                case SkylandersGame.Giants:
                case SkylandersGame.TrapTeam:
                    return _data.EquippedHatGiants;
                default:
                    return _data.EquippedHatSwapForce;
            }
        }

        /// <summary>
        /// Sets the hat worn by the skylander according to a specific game
        /// </summary>
        /// <param name="game">Which game's algorithm should be used for finding the first hat worn</param>
        public void SetHat(SkylandersGame game, Hat hat)
        {
            switch (game)
            {
                case SkylandersGame.SpyrosAdventure:
                    _data.EquippedHatSwapForce = hat;
                    break;
                case SkylandersGame.Giants:
                case SkylandersGame.TrapTeam:
                    _data.EquippedHatGiants = hat;
                    break;
                default:
                    _data.EquippedHatSwapForce = hat;
                    break;
            }
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
        /// The date and time that the figure was built
        /// </summary>
        /// <remarks>
        /// Seconds and minutes are always zero
        /// </remarks>
        public DateTime LastBuildTime => _data.LastBuild;
        
        public ushort HeroPoints
        {
            get { return _data.HeroPoints; }
            set { _data.HeroPoints = value; }
        }
        
        public HeroicChallengesSpyrosAdventure CompletedSpyrosAdventureHeroicChallenges
        {
            get { return _data.CompletedSpyrosAdventureHeroicChallenges; }
            set { _data.CompletedSpyrosAdventureHeroicChallenges = value; }
        }

        public HeroicChallengesGiants CompletedGiantsHeroicChallenges
        {
            get { return _data.CompletedGiantsHeroicChallenges; }
            set { _data.CompletedGiantsHeroicChallenges = value; }
        }

        public TrinketType Trinket
        {
            get { return _data.Trinket; }
            set { _data.Trinket = value; }
        }

        public GiantsQuests CompletedGiantsQuests
        {
            get { return _data.GiantsQuests; }
            set { _data.GiantsQuests = value; }
        }

        public SwapForceQuests CompletedSwapForceQuests
        {
            get { return _data.SwapForceQuests; }
            set { _data.SwapForceQuests = value; }
        }

        public string Nickname
        {
            get { return _data.Nickname ?? ToyName; }
            set { _data.Nickname = value; }
        }

        public ulong OwnerId
        {
            get { return _data.OwnerId; }
            set { _data.OwnerId = value; }
        }

        public BattleGroundFlags BattleGroundFlags
        {
            get { return _data.BattleGroundFlags; }
            set { _data.BattleGroundFlags = value; }
        }

        public Platform Platforms => _data.Platforms;
        
        public byte SwapForceAccoladeRank => _data.SwapForceAccoladeRank;
        public byte GiantsAccoladeRank => _data.GiantsAccoladeRank;

        public bool HasChosenPath => _data.UpgradeData.HasChosenPath;
        public UpgradePath Path => _data.UpgradeData.Path;
        
        /// <summary>
        /// Whether a specific upgrade has been unlocked by index
        /// </summary>
        /// <param name="upgrade">The upgrade to check</param>
        /// <returns>true if the upgrade has been unlocked for the skylander</returns>
        public bool HasUpgrade(Upgrade upgrade)
        {
            return _data.UpgradeData.HasUpgrade(upgrade);
        }
        
        /// <summary>
        /// Sets a specific upgrade's unlock status
        /// </summary>
        /// <param name="upgrade">The upgrade to set</param>
        /// <param name="value">Whether the upgrade should be unlocked or not</param>
        public void SetUpgrade(Upgrade upgrade, bool value)
        {
            _data.UpgradeData = value ? _data.UpgradeData.WithUpgrade(upgrade) :  _data.UpgradeData.WithoutUpgrade(upgrade);
        }
        
        /// <summary>
        /// Sets the current upgrade path
        /// </summary>
        /// <param name="path">Which path to choose</param>
        public void SetUpgradePath(UpgradePath path)
        {
            switch (path)
            {
                case UpgradePath.Top:
                    _data.UpgradeData = _data.UpgradeData.ChooseLeftPath();
                    break;
                case UpgradePath.Bottom:
                    _data.UpgradeData = _data.UpgradeData.ChooseRightPath();
                    break;
                case UpgradePath.None:
                    _data.UpgradeData = _data.UpgradeData.ClearPathUpgrades();
                    break;
            }
        }
    }
        
}