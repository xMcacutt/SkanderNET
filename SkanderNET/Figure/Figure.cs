using System;
using System.IO;
using System.Text;

namespace SkanderNET
{
    
    public class Figure
    {
        protected readonly ToyHeader Header;
        internal readonly FigureSession Session;
        protected readonly byte[] RawData;
        protected readonly ToyMetaData MetaData;
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

        public void DumpFullFigure(string filePath)
        {
            if (_isLoaded)
                Session.DumpFigure(filePath);
        }

        public void DumpDataArea(string filePath)
        {
            if (_isLoaded)
                File.WriteAllBytes(filePath, RawData);
        }

        public void ForceWriteBlockDirect(uint blockIndex, byte[] block)
        {
            Session.ForceWriteBlockDirect(blockIndex, block);
        }
        
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