using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SkanderNET
{
    internal class DataArea
    {
        public readonly byte[] Header;
        public readonly byte[] Data;
        public bool IsValid;

        public DataArea(byte[] header, byte[] data)
        {
            Header = header;
            Data = data;
            IsValid = false;
        }
    }
    
    internal class FigureSession
    {
        private int _activeArea;
        private int _activeExtendedArea;
        private bool _markedForFormat;
        
        private bool _hasExtendedDataArea = false;
        internal readonly Portal Portal;
        private readonly int _slotIndex;
        private ToyHeader _header;
        private readonly byte[] _rawFigure = new byte[0x400];
        private readonly byte[] _rawTag = new byte[0x20];
        private List<DataArea> _dataAreas = new List<DataArea>();
        private List<DataArea> _extendedDataAreas = new List<DataArea>();
        private readonly byte[] _rawData = new byte[0x150];
        
        internal FigureSession(Portal portal, int slotIndex)
        {
            Portal = portal;
            _slotIndex = slotIndex;
        }

        internal void HandleBlock(uint blockIndex, byte[] data)
        {
            Buffer.BlockCopy(data, 0, _rawFigure, (int)blockIndex * 0x10, 0x10);
            if (blockIndex == 0x1)
            {
                Buffer.BlockCopy(_rawFigure, 0x0, _rawTag, 0x0, 0x20);
                _header = Utils.ByteArrayToStruct<ToyHeader>(_rawTag);
                ToyMetaData metaData;
                if (!ToyIndex.Toys.TryGetValue(_header.ToyTypeId, out metaData))
                {
                    Portal.Error(new UnknownToyException($"Unsupported toy with id {_header.ToyTypeId}"));
                    return;
                }
                if (metaData.Type == ToyType.AdventurePack || metaData.Type == ToyType.MagicItem)
                {
                    var figure = this.CreateFigure(_header, _rawFigure);
                    Portal.FigurePlaced(_slotIndex, figure);
                    Portal.FigureProcessed(_slotIndex, figure);
                    return;
                }
                var placeholderFigure = new Figure(this, _header, metaData, null);
                Portal.SetFigure(_slotIndex, placeholderFigure);
                Portal.FigurePlaced(_slotIndex, placeholderFigure);
            }
            if (blockIndex != 0x3F)
                Portal.SendQuery(_slotIndex, blockIndex + 1);
            else
            {
                ToyMetaData metaData;
                if (!ToyIndex.Toys.TryGetValue(_header.ToyTypeId, out metaData))
                    return;
                PreDecryptVerify(metaData);
                DecryptData();
#if DEBUG
                DumpFigure($"./{metaData.Name}");
#endif
                SeparateData();
                if (!VerifyData(metaData.Type) || _markedForFormat)
                {
                    if (!FormatData(metaData))
                    {
                        Portal.Error(new UnfixableCorruptionException(
                            "The toy cannot be formatted with this tool.\n" +
                            "Load any game (Giants or later) and reset broken toys from general settings on the main menu."));
                        return;
                    }
                    Console.WriteLine("Format completed");
                }
                DetermineActiveArea();
                CompileRawData();
                var figure = this.CreateFigure(_header, _rawData);
                Portal.SetFigure(_slotIndex, figure);
                Portal.FigureProcessed(_slotIndex, figure);
            }
        }

        private void PreDecryptVerify(ToyMetaData metaData)
        {
            if (metaData.Type != ToyType.Crystal && metaData.Type != ToyType.Trap)
                _hasExtendedDataArea = true;
            var headerOffsets = _hasExtendedDataArea ? new [] { 0x80, 0x240, 0x110, 0x2d0 } : new [] { 0x80, 0x240 };
            var block = new byte[16];
            var allSegmentsZero = headerOffsets.Length != 0;
            foreach (var offset in headerOffsets)
            {
                Buffer.BlockCopy(_rawFigure, offset, block, 0, 16);
                if (block.All(b => b == 0x0)) continue;
                allSegmentsZero = false;
                break;
            }
            if (allSegmentsZero) 
                _markedForFormat = true;
        }

        internal void MarkForFormat()
        {
            var data = new byte[0x10];
            Portal.SendWrite(_slotIndex, 0x8, data);
            Portal.SendWrite(_slotIndex, 0x24, data);
            if (_hasExtendedDataArea)
            {
                Portal.SendWrite(_slotIndex, 0x11, data);
                Portal.SendWrite(_slotIndex, 0x2d, data);
            }
            Portal.Reset();
        }

        private bool FormatData(ToyMetaData metaData)
        {
            switch (metaData.Type)
            {
                case ToyType.Skylander:
                    SkylanderFigure.Format(_rawFigure);;
                    SeparateData();
                    return VerifyData(metaData.Type);
                case ToyType.Trap:
                    TrapFigure.Format(_rawFigure);;
                    SeparateData();
                    return VerifyData(metaData.Type);
            }
            return false;
        }

        private void CompileRawData()
        {
            if (_hasExtendedDataArea)
            {
                var activeArea = _dataAreas[_activeArea];
                var activeExtendedArea = _extendedDataAreas[_activeExtendedArea];
                var rawAreaBlock = activeArea.Header.Concat(activeArea.Data).ToArray();
                var rawExtendedAreaBlock = activeExtendedArea.Header.Concat(activeExtendedArea.Data).ToArray();
                var fullRawBlock = rawAreaBlock.Concat(rawExtendedAreaBlock).ToArray();
                Buffer.BlockCopy(fullRawBlock, 0, _rawData, 0, fullRawBlock.Length);
            }
            else
            {
                var activeArea = _dataAreas[_activeArea];
                var rawBlock = activeArea.Header.Concat(activeArea.Data).ToArray();
                Buffer.BlockCopy(rawBlock, 0, _rawData, 0, rawBlock.Length);
            }
        }

        private void DetermineActiveArea()
        {
            if (_dataAreas.All(x => x.IsValid))
            {
                var seq1 = _dataAreas[0].Header[0x9];
                var seq2 = _dataAreas[1].Header[0x9];
                _activeArea = ((seq1 - seq2) & 0xFF) < 0x80 ? 0 : 1;
            }
            else
            {
                if (_dataAreas[0].IsValid)
                    _activeArea = 0;
                if (_dataAreas[1].IsValid)
                    _activeArea = 1;
            }

            if (_extendedDataAreas.Any() && _extendedDataAreas.All(x => x.IsValid))
            {
                var seq1 = _extendedDataAreas[0].Header[0x2];
                var seq2 = _extendedDataAreas[1].Header[0x2];
                _activeExtendedArea = ((seq1 - seq2) & 0xFF) < 0x80 ? 0 : 1;
            }
            else if (_extendedDataAreas.Any())
            {
                if (_extendedDataAreas[0].IsValid)
                    _activeExtendedArea = 0;
                if (_extendedDataAreas[1].IsValid)
                    _activeExtendedArea = 1;
            }
        }

        private void DecryptData()
        {
            var block = new byte[0x10];
            for (uint blockIndex = 0x8; blockIndex < 0x40; blockIndex++)
            {
                Buffer.BlockCopy(_rawFigure, (int)blockIndex * 0x10, block, 0x0, 0x10);
                block = FigureEncryption.DecryptBlock(_rawTag, blockIndex, block);
                Buffer.BlockCopy(block, 0x0, _rawFigure, (int)blockIndex * 0x10, 0x10);
            }
        }

        private void SeparateData()
        {
            _dataAreas.Clear();
            _extendedDataAreas.Clear();
            
            if (_hasExtendedDataArea)
            {
                var dataArea1Header = new byte[0x10];
                Buffer.BlockCopy(_rawFigure, 0x80, dataArea1Header, 0x0, 0x10);
                var dataArea1 = new byte[0x60];
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x10, dataArea1, 0x0, 0x20);
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x40, dataArea1, 0x20, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x80, dataArea1, 0x50, 0x10);
                
                var dataArea2Header = new byte[0x10];
                Buffer.BlockCopy(_rawFigure, 0x240, dataArea2Header, 0x0, 0x10);
                var dataArea2 = new byte[0x60];
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x10, dataArea2, 0x0, 0x20);
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x40, dataArea2, 0x20, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x80, dataArea2, 0x50, 0x10);
                
                var dataArea3Header = new byte[0x10];
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x90, dataArea3Header, 0x0, 0x10);
                var dataArea3 = new byte[0x30];
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x90 + 0x10, dataArea3, 0x0, 0x10);
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x90 + 0x30, dataArea3, 0x10, 0x20);
                
                var dataArea4Header = new byte[0x10];
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x90, dataArea4Header, 0x0, 0x10);
                var dataArea4 = new byte[0x30];
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x90 + 0x10, dataArea4, 0x0, 0x10);
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x90 + 0x30, dataArea4, 0x10, 0x20);
                
                _dataAreas.Add(new DataArea(dataArea1Header, dataArea1));
                _dataAreas.Add(new DataArea(dataArea2Header, dataArea2));
                _extendedDataAreas.Add(new DataArea(dataArea3Header, dataArea3));
                _extendedDataAreas.Add(new DataArea(dataArea4Header, dataArea4));
            }
            else
            {
                var dataArea1Header = new byte[0x10];
                Buffer.BlockCopy(_rawFigure, 0x80, dataArea1Header, 0x0, 0x10);
                var dataArea1 = new byte[0x140];
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x10, dataArea1, 0x0, 0x20);
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x40, dataArea1, 0x20, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x80, dataArea1, 0x50, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x80 + 0xC0, dataArea1, 0x80, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x100, dataArea1, 0xB0, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x140, dataArea1, 0xE0, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x80 + 0x180, dataArea1, 0x110, 0x30);
                
                
                var dataArea2Header = new byte[0x10];
                Buffer.BlockCopy(_rawFigure, 0x240, dataArea2Header, 0x0, 0x10);
                var dataArea2 = new byte[0x140];
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x10, dataArea2, 0x0, 0x20);
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x40, dataArea2, 0x20, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x80, dataArea2, 0x50, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x240 + 0xC0, dataArea2, 0x80, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x100, dataArea2, 0xB0, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x140, dataArea2, 0xE0, 0x30);
                Buffer.BlockCopy(_rawFigure, 0x240 + 0x180, dataArea2, 0x110, 0x30);
                
                _dataAreas.Add(new DataArea(dataArea1Header, dataArea1));
                _dataAreas.Add(new DataArea(dataArea2Header, dataArea2));
            }
        }

        private bool VerifyData(ToyType type)
        {
            if (type == ToyType.AdventurePack)
                return true;
            if (type == ToyType.MagicItem)
                return true;
            if (type == ToyType.Skylander)
                return SkylanderFigure.Verify(_dataAreas, _extendedDataAreas);
            if (type == ToyType.Trap)
                return TrapFigure.Verify(_dataAreas);
            return false;
        }
        
        internal void SaveFigure(Figure figure, byte[] rawData)
        {
            var block = new byte[0x10];
            if (_hasExtendedDataArea)
            {
                var startingBlock = _activeArea == 0 ? 0x24 : 0x8;
                var localBlockIndex = 1;
                for (var blockIndex = 0; blockIndex < 0x8; blockIndex++)
                {
                    if ((startingBlock + blockIndex - 3) % 4 == 0)
                        continue;
                    Buffer.BlockCopy(rawData, localBlockIndex * 0x10, block, 0x0, 0x10);
                    localBlockIndex++;
                    block = FigureEncryption.EncryptBlock(_rawTag, (uint)(startingBlock + blockIndex + 0x1), block);
                    Portal.SendWrite(_slotIndex, (uint)(startingBlock + blockIndex + 0x1), block);
                }
                Buffer.BlockCopy(rawData, 0x0, block, 0x0, 0x10);
                block = FigureEncryption.EncryptBlock(_rawTag, (uint)startingBlock, block);
                Portal.SendWrite(_slotIndex, (uint)startingBlock, block);
                _activeArea = _activeArea == 0 ? 1 : 0;
                
                localBlockIndex = 1;
                startingBlock = _activeExtendedArea == 0 ? 0x2d : 0x11;
                for (var blockIndex = 0; blockIndex < 0x4; blockIndex++)
                {
                    if ((startingBlock + blockIndex - 3) % 4 == 0)
                        continue;
                    Buffer.BlockCopy(rawData, 0x70 + localBlockIndex * 0x10, block, 0x0, 0x10);
                    localBlockIndex++;
                    block = FigureEncryption.EncryptBlock(_rawTag, (uint)(startingBlock + blockIndex + 0x1), block);
                    Portal.SendWrite(_slotIndex, (uint)(startingBlock + blockIndex + 0x1), block);
                }
                Buffer.BlockCopy(rawData, 0x70, block, 0x0, 0x10);
                block = FigureEncryption.EncryptBlock(_rawTag, (uint)startingBlock, block);
                Portal.SendWrite(_slotIndex, (uint)startingBlock, block);
                _activeExtendedArea = _activeExtendedArea == 0 ? 1 : 0;
            }
            else
            {
                var startingBlock = _activeArea == 0 ? 0x24 : 0x8;
                var localBlockIndex = 1;
                for (var blockIndex = 0; blockIndex < 0x1A; blockIndex++)
                {
                    if ((startingBlock + blockIndex - 3) % 4 == 0)
                        continue;
                    Buffer.BlockCopy(rawData, localBlockIndex * 0x10, block, 0x0, 0x10);
                    localBlockIndex++;
                    block = FigureEncryption.EncryptBlock(_rawTag, (uint)(startingBlock + blockIndex + 1), block);
                    Portal.SendWrite(_slotIndex, (uint)(startingBlock + blockIndex + 1), block);
                }
                Buffer.BlockCopy(rawData, 0x0, block, 0x0, 0x10);
                block = FigureEncryption.EncryptBlock(_rawTag, (uint)startingBlock, block);
                Portal.SendWrite(_slotIndex, (uint)startingBlock, block);
                _activeArea = _activeArea == 0 ? 1 : 0;
            }
            Portal.FigureSaved(_slotIndex, figure);
        }

        internal void DumpFigure(string filePath)
        {
            File.WriteAllBytes(filePath, _rawFigure);
        }
    }
}