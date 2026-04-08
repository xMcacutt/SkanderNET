namespace SkanderNET.Data
{
    public struct BattleGroundFlags
    {
        private uint _flags;

        internal BattleGroundFlags(uint flags)
        {
            _flags = flags;
        }
        
        internal uint Flags
        {
            get { return _flags; }
            set { _flags = value; } 
        }

        internal byte PortalMasterLevel
        {
            get
            {
                return (byte)((_flags >> 0x1A) & 0x3F);
            }
            set
            {
                _flags = _flags & ~(0x3F << 0x1A) | ((uint)value & 0x3F) << 0x1A;
            }
        }

        internal byte AbilitySlotCount
        {
            get
            {
                return (byte)(((_flags >> 0x18) & 0x3) + 1);
            }
            set
            {
                _flags = (_flags & ~(0x3u << 0x18)) | ((uint)(value - 1) & 0x3) << 0x18;
            }
        }
            
            internal byte GetAbilityLevel(int index)
        {
            if (index < 0 || index > 7)
                return 0;
            return (byte)((_flags >> (index * 3)) & 0x7);
        }

        internal void SetAbilityLevel(int index, int level)
        {
            if (index < 0 || index > 7)
                return;
            _flags = (_flags & ~(0x7u << (index * 3))) | ((uint)level & 0x7) << (index * 3);
        }
    }
}