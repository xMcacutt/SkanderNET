using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SkanderNET
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SkylanderDataHeader
    {
        internal fixed byte ExperienceLevelBytes[3];
        internal ushort Money;
        internal uint PlayTimeInSeconds;
        internal byte AreaSequenceValue;
        internal ushort Crc16Type3;
        internal ushort Crc16Type2;
        internal ushort Crc16Type1;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SkylanderMetaData
    {
        internal uint Serial;
        private uint unk0x4;
        private uint unk0x8;
        private uint unk0xC;
        internal ushort CharacterId;
        private ushort unk0x12;
        internal ulong TradingCardId;
        internal ushort VariantId;
        private ushort crc16Type0;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SkylanderDataArea
    {
        // Block 1
        internal ushort SkillFlags;
        private byte unk0x92;
        internal byte PlatformFlags;
        internal ushort HatId;
        internal ushort CanUseThirdAbility;
        private ulong unk0x98;
        // Block 2
        internal fixed byte NicknamePart1[0x10];
        // Block 3
        internal fixed byte NicknamePart2[0x10];
        // Block 4
        internal byte LastSaveMinute;
        internal byte LastSaveHour;
        internal byte LastSaveDay;
        internal byte LastSaveMonth;
        internal ushort LastSaveYear;
        internal uint CompletedHeroicChallengesFlags;
        internal ushort HeroPoints;
        private byte unkFlag0xDC;
        private byte unkFlag0xDD;
        private byte unkFlag0xDE;
        private byte unkBool0xDF;
        // Block 5
        internal byte LastResetMinute;
        internal byte LastResetHour;
        internal byte LastResetDay;
        internal byte LastResetMonth;
        internal ushort LastResetYear;
        private fixed byte padding0xE6[0xA];
        // Block 6
        private fixed byte unkBlock0x10[0x10];
    }

    public class Skylander
    {
        private readonly Portal _portal;
        private readonly int _slotIndex;
        private SkylanderMetaData _metaData;
        private readonly byte[] _rawMeta = new byte[0x20];
        private readonly byte[] _rawLoadedData = new byte[0x140];
        private SkylanderDataHeader _area1Header;
        private SkylanderDataHeader _area2Header;
        private SkylanderDataHeader _activeHeader;
        private SkylanderDataArea _activeData;
        private int _receivedDataBlocks = 0;
        private int _activeArea;
        private uint[] _activeDataAreaIds;
        private bool _loaded;
        private readonly byte[] _fullRawData = new byte[0x400];
        private static readonly uint[] BlockIdsArea1 = { 0x09, 0x0A, 0x0C, 0x0D, 0x0E, 0x10 };
        private static readonly uint[] BlockIdsArea2 = { 0x25, 0x26, 0x28, 0x29, 0x2A, 0x2C };

        internal Skylander(Portal portal, int slotIndex) { _portal = portal; _slotIndex = slotIndex; }
        
        /// <summary>
        /// Attempts to get the <see cref="SkylanderGenericData"/> entry in the <see cref="SkylanderIndex"/> lookup providing the name, element, and type of Skylander.
        /// </summary>
        /// <param name="genericData">Upon return, contains the <see cref="SkylanderGenericData"/> if found and null otherwise.</param>
        /// <returns><c>true</c> if the metadata was successfully retrieved. Otherwise, <c>false</c>.</returns>
        public bool TryGetMetaData(out SkylanderGenericData genericData)
        {
            genericData = SkylanderIndex.Skylanders.FirstOrDefault(x =>
                             x.Id == _metaData.CharacterId && x.Variant == _metaData.VariantId)
                         ?? SkylanderIndex.Skylanders.FirstOrDefault(x => x.Id == _metaData.CharacterId);
            return genericData != null;
        }

        private void EnsureLoaded()
        {
            if (!_loaded)
                throw new SkylanderNotLoadedException(
                    "Skylander was read from or written to before being fully loaded. " +
                    "Subscribe to Portal.OnSkylanderProcessed to guarantee loading success.");
        }
        
        private void SyncData()
        {
            var bytes = Utils.StructToByteArray(_activeData);
            Buffer.BlockCopy(bytes, 0, _rawLoadedData, 0, bytes.Length);
        }
        
        private void DetermineActiveArea()
        {
            _activeArea = ((_area1Header.AreaSequenceValue - _area2Header.AreaSequenceValue) & 0xFF) < 0x80 ? 1 : 2;
            _activeHeader = _activeArea == 1 ? _area1Header : _area2Header;
            _activeDataAreaIds = _activeArea == 1 ? BlockIdsArea1 : BlockIdsArea2;
        }

#if DEBUG
        public void DebugRewriteCharacter(SkylanderGenericData data)
        {
            if (!DebugGetAllBlocks)
                return;
            Buffer.BlockCopy(BitConverter.GetBytes(data.Id), 0, _fullRawData, 0x10, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(data.Variant), 0, _fullRawData, 0x1C, 2);
            var crc = CRC16_IBM3740.Generate(_fullRawData.Take(0x1E).ToArray());
            Buffer.BlockCopy(BitConverter.GetBytes(crc), 0, _fullRawData, 0x1E, 2);
            var sector0 = _fullRawData.Take(0x20).ToArray();
            for (uint i = 0; i < 0x40; i++)
            {
                var unencryptedBlock = _fullRawData.Skip(0x10 * (int)i).Take(0x10).ToArray();
                var block = SkylanderEncryption.EncryptBlock(sector0, i, unencryptedBlock);
                _portal.SendWrite(_slotIndex, i, block);
            }
        }

        
        public void DebugRewriteCharacter(byte[] rawData)
        {
            if (!DebugGetAllBlocks)
                return;
            var sector0 = rawData.Take(0x20).ToArray();
            for (uint i = 0; i < 0x40; i++)
            {
                var unencryptedBlock = rawData.Skip(0x10 * (int)i).Take(0x10).ToArray();
                var block = SkylanderEncryption.EncryptBlock(sector0, i, unencryptedBlock);
                _portal.SendWrite(_slotIndex, i, block);
            }
        }
#endif

        private const bool DebugGetAllBlocks = true;
        internal void HandleBlock(uint blockIndex, byte[] data)
        {
            if (blockIndex < 0x2)
                Buffer.BlockCopy(data, 0, _rawMeta, (int)(blockIndex * 0x10), 0x10);
            if (blockIndex == 0x0)
                _portal.SendQuery(_slotIndex, 0x1);
            if (blockIndex == 0x1)
            {
                _metaData = Utils.ByteArrayToStruct<SkylanderMetaData>(_rawMeta);
                _portal.SkylanderPlaced(_slotIndex, this);
                if (!DebugGetAllBlocks)
                    _portal.SendQuery(_slotIndex, 0x8);
            }
            if (DebugGetAllBlocks)
            {
                var block = SkylanderEncryption.DecryptBlock(_rawMeta, blockIndex, data);
                Buffer.BlockCopy(block, 0, _fullRawData, (int)(blockIndex * 0x10), 0x10);
                if (blockIndex < 0x3F && blockIndex >= 1)
                    _portal.SendQuery(_slotIndex, blockIndex + 1);
                else if (blockIndex == 0x3F)
                {
                    _portal.SkylanderProcessed(_slotIndex, this); 
                    DumpFigure();
                }
                return;
            }
            if (blockIndex == 0x08)
            {
                var block = SkylanderEncryption.DecryptBlock(_rawMeta, blockIndex, data);
                _area1Header = Utils.ByteArrayToStruct<SkylanderDataHeader>(block);
                _portal.SendQuery(_slotIndex, 0x24);
            }
            else if (blockIndex == 0x24)
            {
                var block = SkylanderEncryption.DecryptBlock(_rawMeta, blockIndex, data);
                _area2Header = Utils.ByteArrayToStruct<SkylanderDataHeader>(block);
                DetermineActiveArea();
                _portal.SendQuery(_slotIndex, _activeDataAreaIds[0]);
            }
            else if (blockIndex > 0x08 && blockIndex % 4 != 3)
            {
                var block = SkylanderEncryption.DecryptBlock(_rawMeta, blockIndex, data);
                var blockSubIndex = Array.IndexOf(_activeDataAreaIds, blockIndex);
                if (blockSubIndex < 0)
                    throw new Exception("Invalid block index was read on Skylander. This shouldn't be possible.");
                Buffer.BlockCopy(block, 0, _rawLoadedData, blockSubIndex * 0x10, 0x10);
                _receivedDataBlocks++;
                if (_receivedDataBlocks != _activeDataAreaIds.Length)
                {
                    if (_activeDataAreaIds == null ||
                        (!BlockIdsArea1.Contains(blockIndex) && !BlockIdsArea2.Contains(blockIndex)))
                        return;
                    var index = Array.IndexOf(_activeDataAreaIds, blockIndex);
                    if (index >= 0 && index < _activeDataAreaIds.Length - 1)
                        _portal.SendQuery(_slotIndex, _activeDataAreaIds[index + 1]);
                    return;
                }
                _activeData = Utils.ByteArrayToStruct<SkylanderDataArea>(_rawLoadedData);
                _loaded = true;
                _portal.SkylanderProcessed(_slotIndex, this);
            }
        }

        private void DumpFigure()
        {
            if (!DebugGetAllBlocks)
                return;
            SkylanderGenericData data;
            if (TryGetMetaData(out data))
                File.WriteAllBytes($"./{data.Name}", _fullRawData);
        }
        
        private bool GenerateChecksums()
        {
            EnsureLoaded();
            var meta = new byte[0x1E];
            Buffer.BlockCopy(_rawMeta, 0, meta, 0, 0x1E);
            var crc16Type0 = CRC16_IBM3740.Generate(meta);
            var dataArea = new List<byte>();
            dataArea.AddRange(_rawLoadedData.Take(0x60));
            dataArea.AddRange(new byte[0xE0]);
            var crc16Type2Area = CRC16_IBM3740.Generate(dataArea.Take(0x30).ToArray());
            var crc16Type3Area = CRC16_IBM3740.Generate(dataArea.Skip(0x30).ToArray());
            var headerBlock = Utils.StructToByteArray(_activeHeader);
            BitConverter.GetBytes(crc16Type3Area).CopyTo(headerBlock, 0xA);
            BitConverter.GetBytes(crc16Type2Area).CopyTo(headerBlock, 0xC);
            headerBlock[0xE] = 0x05;
            headerBlock[0xF] = 0x00;
            headerBlock[0x9]++;
            var crc16Type1Area = CRC16_IBM3740.Generate(headerBlock);
            BitConverter.GetBytes(crc16Type1Area).CopyTo(headerBlock, 0xE);
            _activeHeader = Utils.ByteArrayToStruct<SkylanderDataHeader>(headerBlock);
            if (_activeArea == 1)
                _area1Header = _activeHeader;
            else
                _area2Header = _activeHeader;
            return true;
        }

        /// <summary>
        /// Saves the data modified back to the Skylander. Returns early if no Skylander is loaded.
        /// </summary>
        /// <exception cref="ChecksumGenerationFailureException">Thrown on a failure to generate the checksums needed to save data back to the Skylander. No data will be written to the Skylander if this exception is thrown.</exception>
        /// <exception cref="SkylanderNotLoadedException">Thrown on attempting to read/write to/from the Skylander before it is fully loaded.</exception>
        public void Save()
        {
            EnsureLoaded();
            SyncData();
            if (!GenerateChecksums())
                throw new ChecksumGenerationFailureException("Saving skylander failed, checksum generation was unsuccessful.");
            var dataBlockIds = _activeArea == 1 ? BlockIdsArea2 : BlockIdsArea1;
            for (var i = 0; i < dataBlockIds.Length; i++)
            {
                var data = new byte[0x10];
                Buffer.BlockCopy(_rawLoadedData, i * 0x10, data, 0, 0x10);
                var encryptedData = SkylanderEncryption.EncryptBlock(_rawMeta, dataBlockIds[i], data);
                _portal.SendWrite(_slotIndex, dataBlockIds[i], encryptedData);
            }
            var headerData = Utils.StructToByteArray(_activeHeader);
            var headerBlockId = _activeArea == 1 ? (uint)0x24 : 0x08;
            var encryptedHeaderData = SkylanderEncryption.EncryptBlock(_rawMeta, headerBlockId, headerData);
            _portal.SendWrite(_slotIndex, headerBlockId, encryptedHeaderData);

            _activeArea = _activeArea == 1 ? 2 : 1;
            _activeHeader = _activeArea == 1 ? _area1Header : _area2Header;
            _activeDataAreaIds = _activeArea == 1 ? BlockIdsArea1 : BlockIdsArea2;
            _portal.SkylanderSaved(_slotIndex, this);
        }
        
        /// <summary>
        /// Unique identifier for the specific instance of the figure.
        /// </summary>
        public uint SerialNumber => _metaData.Serial;
        
        /// <summary>
        /// Unique identifier for the character.
        /// </summary>
        public ushort CharacterId => _metaData.CharacterId;
        
        /// <summary>
        /// Unique identifier for the trading card tied to the figure.
        /// </summary>
        public ulong TradingCardId => _metaData.TradingCardId;
        
        /// <summary>
        /// Unique identifier for the variant of the character.
        /// </summary>
        public ulong VariantId => _metaData.VariantId;
        
        /// <summary>
        /// Amount of experience points obtained by the figure.
        /// </summary>
        public unsafe uint ExperienceLevel
        {
            get
            {
                EnsureLoaded();
                fixed (byte* b = _activeHeader.ExperienceLevelBytes)
                {
                    return (uint)(b[0] | (b[1] << 8) | (b[2] << 16));
                }
            }
            set
            {
                EnsureLoaded();
                fixed (byte* b = _activeHeader.ExperienceLevelBytes)
                {
                    b[0] = (byte)(value & 0xFF);
                    b[1] = (byte)((value >> 8) & 0xFF);
                    b[2] = (byte)((value >> 16) & 0xFF);
                }
                SyncData();
            }
        }
        
        /// <summary>
        /// Amount of money obtained by the figure.
        /// </summary>
        public ushort Money
        {
            get
            {
                EnsureLoaded();
                return _activeHeader.Money;
            }
            set
            {
                EnsureLoaded();
                _activeHeader.Money = value;
                SyncData();
            }
        }
        
        /// <summary>
        /// Time spent playing as the figure in seconds.
        /// </summary>
        public uint PlayTimeInSeconds => _activeHeader.PlayTimeInSeconds;
        
        private SkillData Skills
        {
            get
            {
                EnsureLoaded();
                return new SkillData(_activeData.SkillFlags);
            }
            set
            {
                EnsureLoaded();
                _activeData.SkillFlags = value.Raw;
                SyncData();
            }
        }

        /// <summary>
        /// Unlocks a specific skill for the character. Must be saved back to the figure with <see cref="Save"/>.
        /// </summary>
        /// <param name="index">Index of the skill to unlock.</param>
        public void UnlockUpgrade(int index)
        {
            EnsureLoaded();
            Skills = Skills.WithUpgrade(index);
        }

        /// <summary>
        /// Locks a specific upgrade for the character. Must be saved back to the figure with <see cref="Save"/>..
        /// </summary>
        /// <param name="index">Index of the skill to lock.</param>
        public void RemoveUpgrade(int index)
        {
            EnsureLoaded();
            Skills = Skills.WithoutUpgrade(index);
        }

        /// <summary>
        /// Sets the upgrade path for the figure. Must be saved back to the figure with <see cref="Save"/>.
        /// </summary>
        /// <param name="path"><see cref="SkillPath"/> for either left or right path.</param>
        public void SetPath(SkillPath path)
        {
            EnsureLoaded();
            Skills = path == SkillPath.Left ? Skills.ChooseLeftPath() : Skills.ChooseRightPath();
        }
        
        /// <summary>
        /// Identifier for the hat which the skylander is currently wearing.
        /// Can be used with <see cref="HatIndex"/> to find the name of the hat for Spyro's Adventure and Giants.
        /// Sometimes hats don't seem to save to the documented location. Wish I knew why.
        /// </summary>
        public ushort HatId
        {
            get
            {
                EnsureLoaded();
                return _activeData.HatId;
            }
            set
            {
                EnsureLoaded();
                _activeData.HatId = value;
                SyncData();
            }
        }

        /// <summary>
        /// Determines if the third action / ability is purchased and usable by the character.
        /// </summary>
        public bool CanUseThirdAbility
        {
            get
            {
                EnsureLoaded();
                return _activeData.CanUseThirdAbility == 1;
            }
            set
            {
                EnsureLoaded();
                _activeData.CanUseThirdAbility = value ? (ushort)1 : (ushort)0;
                SyncData();
            }
        }
        
        /// <summary>
        /// Gets or sets the display name of the figure.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When getting the value:
        /// </para>
        /// <list type="number">
        /// <item>
        /// Returns the nickname stored on the figure if it contains a valid value.
        /// </item>
        /// <item>
        /// Otherwise returns the default character name from <see cref="SkylanderIndex"/> if metadata is available.
        /// </item>
        /// <item>
        /// Otherwise returns <c>"Unknown"</c>.
        /// </item>
        /// </list>
        /// <para>
        /// When setting the value, the value specified becomes the nickname of the Skylander.
        /// Max length of 14 characters.
        /// Must be saved back to the figure with <see cref="Save"/>.
        /// </para>
        /// </remarks>
        public unsafe string Name
        {
            get
            {
                const int partSize = 0x10;
                const int totalSize = partSize * 2;
        
                var buffer = new byte[totalSize];
        
                fixed (byte* p1 = _activeData.NicknamePart1)
                fixed (byte* p2 = _activeData.NicknamePart2)
                {
                    for (var i = 0; i < partSize; i++)
                        buffer[i] = p1[i];
        
                    for (var i = 0; i < partSize; i++)
                        buffer[i + partSize] = p2[i];
                }
                
                var str = Encoding.Unicode.GetString(buffer, 0, 30).Trim();
                SkylanderGenericData genericData; 
                TryGetMetaData(out genericData);
                if (str.Any(c => !char.IsLetterOrDigit(c) && !char.IsPunctuation(c) && !char.IsWhiteSpace(c) && c != 0x0) || str.All(c => char.IsWhiteSpace(c) || c == 0x0))
                    return genericData?.Name ?? "Unknown";
        
                var nullIndex = str.IndexOf('\0');
                if (nullIndex >= 0)
                    str = str.Substring(0, nullIndex);
        
                return str;
            }
            set
            {
                EnsureLoaded();
                const int partSize = 0x10;
                const int totalSize = partSize * 2;

                var buffer = new byte[totalSize];

                var encoded = Encoding.Unicode.GetBytes(value ?? "");

                var length = Math.Min(encoded.Length, 30);
                Array.Copy(encoded, buffer, length);

                fixed (byte* p1 = _activeData.NicknamePart1)
                fixed (byte* p2 = _activeData.NicknamePart2)
                {
                    for (var i = 0; i < partSize; i++)
                        p1[i] = buffer[i];

                    for (var i = 0; i < partSize; i++)
                        p2[i] = buffer[i + partSize];
                }
            }
        }

        /// <summary>
        /// Bitfield for completed heroic challenges.
        /// </summary>
        public uint CompletedHeroicChallengesFlags
        {
            get
            {
                EnsureLoaded();
                return _activeData.CompletedHeroicChallengesFlags;
            }
            set
            {
                EnsureLoaded();
                _activeData.CompletedHeroicChallengesFlags = value;
            }
        }

        /// <summary>
        /// Number of Hero Points obtained.
        /// </summary>
        public ushort HeroPoints
        {
            get
            {
                EnsureLoaded();
                return _activeData.HeroPoints;
            }
            set
            {
                EnsureLoaded();
                _activeData.HeroPoints = value;
            }
        }

        /// <summary>
        /// <see cref="DateTime"/> <see cref="object"/> containing the last time the figure was saved by any game.
        /// </summary>
        public DateTime LastSaveTime
        {
            get
            {
                EnsureLoaded();
                var data = _activeData;
                return new DateTime(
                    data.LastSaveYear, 
                    data.LastSaveMonth, 
                    data.LastSaveDay, 
                    data.LastSaveHour, 
                    data.LastSaveMinute, 
                    0);
            }
        }
        
        /// <summary>
        /// <see cref="DateTime"/> <see cref="object"/> containing the last time the figure was reset.
        /// </summary>
        public DateTime LastResetTime
        {
            get
            {
                EnsureLoaded();
                var data = _activeData;
                return new DateTime(
                    data.LastResetYear, 
                    data.LastResetMonth, 
                    data.LastResetDay, 
                    data.LastResetHour, 
                    data.LastResetMinute, 
                    0);
            }
        }
    }
}