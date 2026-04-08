using System;
using System.IO;
using System.Text;
using SkanderNET.Data;

namespace SkanderNET.Figures
{
    public class Figure
    {
        internal readonly ToyHeader Header;
        internal readonly FigureSession Session;
        internal readonly byte[] RawData;
        internal readonly ToyMetaData MetaData;
        private bool _isLoaded = false;
        private const string WebCodeAlphabet = "23456789BCDFGHJKLMNPQRSTVWXYZ";
        
        internal Figure(FigureSession session, ToyHeader header, ToyMetaData metaData, byte[] rawData)
        {
            Session = session;
            Header = header;
            MetaData = metaData;
            RawData = rawData;
            _isLoaded = RawData != null;
        }

        internal Figure Clone() => new Figure(null, Header, MetaData, null);

        public override string ToString()
        {
            return $"{ToyType}: {ToyName}";
        }

        /// <summary>
        /// Writes the entire loaded figure to a specified file path
        /// </summary>
        /// <param name="filePath">The path to write the file to (overwrites existing files)</param>
        /// <param name="encrypted">Whether the figure should be dumped encrypted or unencrypted</param>
        public void DumpFullFigure(string filePath, bool encrypted)
        {
            if (_isLoaded)
                Session.DumpFigure(filePath, encrypted);
        }

        /// <summary>
        /// Writes the data area of the loaded figure to a specified file path
        /// </summary>
        /// <param name="filePath">The path to write the file to (overwrites existing files)</param>
        public void DumpDataArea(string filePath)
        {
            if (_isLoaded)
                File.WriteAllBytes(filePath, RawData);
        }

        /// <summary>
        /// Writes data directly to the figure at a specified block
        /// </summary>
        /// <param name="blockIndex">The index of the block to write to</param>
        /// <param name="block">The data to write to the block</param>
        public void ForceWriteBlockDirect(uint blockIndex, byte[] block)
        {
            Session.ForceWriteBlockDirect(blockIndex, block);
        }
        
        /// <summary>
        /// Encrypts then writes data directly to the figure at a specified block
        /// </summary>
        /// <param name="blockIndex">The index of the block to write to</param>
        /// <param name="block">The data to write to the block</param>
        public void ForceWriteBlock(uint blockIndex, byte[] block)
        {
            Session.ForceWriteBlock(blockIndex, block);
        }
        
        public uint SerialNumber => Header.SerialNumber;
        public Toy Toy => Header.Toy;
        public string ToyName => MetaData.Name;
        public VariantInfo Variant => new VariantInfo(Header.VariantId);
        public Element Element => MetaData.Element;
        public ToyType ToyType => MetaData.Type;
        public ToySubType ToySubType => MetaData.SubType;
        public ulong TradingCardId => Header.TradingCardId;
        public string WebCode
        {
            get
            {
                if (Header.TradingCardId == 0) return null;
                var code = new StringBuilder();
                var tempId = Header.TradingCardId;
                while (tempId > 0)
                {
                    var remainder = (int)(tempId % 29);
                    code.Insert(0, WebCodeAlphabet[remainder]);
                    tempId /= 29;
                }
                return code.ToString();
            }
        }
    }
}