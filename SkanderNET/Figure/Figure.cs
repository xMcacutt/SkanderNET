using System;
using System.IO;

namespace SkanderNET
{
    
    public class Figure
    {
        protected readonly ToyHeader Header;
        internal readonly FigureSession Session;
        protected readonly byte[] RawData;
        protected readonly ToyMetaData MetaData;
        
        public uint SerialNumber => Header.SerialNumber;
        
        public ushort ToyTypeId => Header.ToyTypeId;
        public string ToyName => MetaData.Name;
        
        public VariantInfo Variant => new VariantInfo(Header.VariantId);
        
        public Element Element => MetaData.Element;
        
        public ToyType ToyType => MetaData.Type;
        
        public ToySubType ToySubType => MetaData.SubType;
        private bool _isLoaded = false;
        
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
    }
}