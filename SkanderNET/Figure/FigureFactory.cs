using System.Linq;

namespace SkanderNET
{
    internal static class FigureFactory
    {
        internal static Figure CreateFigure(this FigureSession session, TagHeader header, byte[] rawFigure)
        {
            ToyMetaData metaData;
            if (!ToyIndex.Toys.TryGetValue(header.ToyTypeId, out metaData))
                return null;
            switch (metaData.Type)
            {
                case ToyType.Skylander:
                    return new FigureSkylander(session, header, metaData, rawFigure);
                case ToyType.AdventurePack:
                    break;
                case ToyType.Crystal:
                    return new CreationCrystalFigure(session, header, metaData, rawFigure);
                case ToyType.MagicItem:
                    break;
                case ToyType.Trap:
                    return new TrapFigure(session, header, metaData, rawFigure);
                case ToyType.Vehicle:
                    return new VehicleFigure(session, header, metaData, rawFigure);
            }

            return null;
        }
    }
}