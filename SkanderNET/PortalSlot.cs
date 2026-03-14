using System;

namespace SkanderNET
{
    public class PortalSlot
    {
        public int Index { get; set; }
        public int Status { get; set; }
        public Skylander CurrentSkylander { get; set; }
        public uint PendingBlock = uint.MaxValue;
        public DateTime LastQueryTime;
        public int RetryCount;
    }
}