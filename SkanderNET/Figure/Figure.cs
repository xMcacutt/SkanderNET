using System;
using System.IO;

namespace SkanderNET
{
    
    public class Figure
    {
        protected readonly TagHeader Header;
        internal readonly FigureSession Session;
        protected readonly byte[] RawData;
        protected readonly ToyMetaData MetaData;
        
        public uint SerialNumber => Header.SerialNumber;
        
        public ushort ToyTypeId => Header.ToyTypeId;
        public string ToyName => MetaData.Name;
        
        /// <summary>
        /// Information about the variant of the figure.
        /// </summary>
        public VariantInfo Variant => new VariantInfo(Header.VariantId);
        
        public Element Element => MetaData.Element;
        
        public ToyType ToyType => MetaData.Type;
        
        public ToySubType ToySubType => MetaData.SubType;
        
        internal Figure(FigureSession session, TagHeader header, ToyMetaData metaData, byte[] rawData)
        {
            Session = session;
            Header = header;
            MetaData = metaData;
            RawData = rawData;
        }

        internal Figure Clone() => new Figure(null, Header, MetaData, null);

        public virtual void Save() {}

        public override string ToString()
        {
            return $"{ToyType}: {ToyName}";
        }

        public void DumpDataArea(string filePath)
        {
            File.WriteAllBytes(filePath, RawData);
        }
    }
}