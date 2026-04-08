using System.Linq;
using SkanderNET.Data;

namespace SkanderNET.Figures
{
    internal static class FigureFactory
    {
        internal static Figure CreateFigure(this FigureSession session, ToyHeader header, byte[] rawFigure)
        {
            ToyMetaData metaData;
            if (!ToyIndex.Toys.TryGetValue(header.Toy, out metaData))
                return null;
            switch (metaData.Type)
            {
                case ToyType.Skylander:
                    return new SkylanderFigure(session, header, metaData, rawFigure);
                case ToyType.AdventurePack:
                    return new AdventurePackFigure(session, header, metaData, rawFigure);
                case ToyType.Crystal:
                    return new CreationCrystalFigure(session, header, metaData, rawFigure);
                case ToyType.MagicItem:
                    return new MagicItemFigure(session, header, metaData, rawFigure);
                case ToyType.Trap:
                    return new TrapFigure(session, header, metaData, rawFigure);
                case ToyType.Vehicle:
                    return new VehicleFigure(session, header, metaData, rawFigure);
                case ToyType.RacingPack:
                    return new RacePackFigure(session, header, metaData, rawFigure);
            }
            return null;
        }
    }
}