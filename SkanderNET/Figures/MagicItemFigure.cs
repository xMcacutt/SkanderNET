using SkanderNET.Data;

namespace SkanderNET.Figures
{
    /// <summary>
    /// Figure that activates some ability or unlock in the games but stores no data
    /// </summary>
    public class MagicItemFigure : Figure
    {
        internal MagicItemFigure(FigureSession session, ToyHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
        }
    }
}