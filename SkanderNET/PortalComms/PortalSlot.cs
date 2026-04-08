using System;
using SkanderNET.Figures;

namespace SkanderNET.PortalComms
{
    internal class PortalSlot
    {
        internal readonly object Sync = new object();
        internal int Index { get; set; }
        internal int Status { get; set; }
        internal FigureSession CurrentFigureSession { get; set; }
        internal Figure CurrentFigure { get; set; }
        internal uint PendingBlock = uint.MaxValue;
        internal DateTime LastQueryTime;
        internal int RetryCount;
        internal bool IsLoaded;
    }
}