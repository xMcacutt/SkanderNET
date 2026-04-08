using SkanderNET.Data;

namespace SkanderNET.Figures
{
    /// <summary>
    /// Figure that unlocks levels in the games
    /// </summary>
    public class AdventurePackFigure : Figure
    {
        internal AdventurePackFigure(FigureSession session, ToyHeader header, ToyMetaData metaData, byte[] rawData) : base(session, header, metaData, rawData)
        {
        }
    }
}