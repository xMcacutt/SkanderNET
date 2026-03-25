using System;

namespace SkanderNET
{
    public class PortalSlot
    {
        public int Index { get; set; }
        public int Status { get; set; }
        internal FigureSession CurrentFigureSession { get; set; }
        public Figure CurrentFigure { get; set; }
        public uint PendingBlock = uint.MaxValue;
        public DateTime LastQueryTime;
        public int RetryCount;
    }
}