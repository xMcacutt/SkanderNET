namespace SkanderNET
{
    public enum Deco : byte
    {
        Normal = 0,
        Repose1 = 1,
        AlternateDeco = 2,
        Legendary = 3,
        Event = 4,
        Repose2 = 5,
        LightDirect = 6,
        LightStored = 7,
        LightEnhanced = 8,
        Repose3 = 9,
        Repose4 = 10,
        Repose5 = 11,
        Valentines = 12,
        Easter = 13,
        Winter = 14,
        Virtual = 15,
        Premium = 16,
        GlowDark = 17,
        StoneStatue = 18,
        GlitterSpectrum = 19,
        TreasureHunt2012 = 20,
        Halloween = 21,
        TreasureHunt2013 = 22,
        ColorShift = 23,
        WiiU = 24,
        TH_BestBuy = 25,
        TH_FritoLay = 26,
        TreasureHunt2014 = 29,
        TreasureHunt2015 = 30,
        Mobile = 31
    }
    
    public enum SkylandersGame : byte
    {
        SpyrosAdventure = 0,
        Giants = 1,
        SwapForce = 2,
        TrapTeam = 3,
        Superchargers = 4,
        Imaginators = 5,
        Skylanders2017 = 6
    }
    
    public struct VariantInfo
    {
        public Deco DecoType { get; }
        public bool IsSupercharger { get; }
        public bool IsLightCore { get; }
        public bool IsInGameVariant { get; }
        public bool IsReposed { get; }
        public SkylandersGame Game { get; }

        internal VariantInfo(ushort raw)
        {
            DecoType = (Deco)(raw & 0xFF);
            IsSupercharger = (raw & (1 << 8)) != 0;
            IsLightCore = (raw & (1 << 9)) != 0;
            IsInGameVariant = (raw & (1 << 10)) != 0;
            IsReposed = (raw & (1 << 11)) != 0;
            Game = (SkylandersGame)((raw >> 12) & 0xF);
        }

        public ushort ToUInt16()
        {
            ushort value = 0;
            value |= (byte)DecoType;
            if (IsSupercharger)  value |= (1 << 8);
            if (IsLightCore)     value |= (1 << 9);
            if (IsInGameVariant) value |= (1 << 10);
            if (IsReposed)       value |= (1 << 11);
            value |= (ushort)((byte)Game << 12);
            return value;
        }
    }
}