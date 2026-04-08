using System;

namespace SkanderNET.Data
{
    [Flags]
    public enum TrophyVillain : ushort
    {
        None = 0,
        Glumshanks = 1 << 0x0,
        DragonHunter = 1 << 0x1,
        MoneyBone = 1 << 0x2,
        ChompyMage = 1 << 0x3,
        DrKrankcase = 1 << 0x4,
        Mesmerelda = 1 << 0x5,
        CaptainFrightbeard = 1 << 0x6,
        GoldenQueen = 1 << 0x7,
        Spellslamzer = 1 << 0x8,
        TheGulper = 1 << 0x9,
        ChefPepperJack = 1 << 0xA,
        Stratosphere = 1 << 0xB,
        CapNCluck = 1 << 0xC,
        Wolfgang = 1 << 0xD,
        PainYatta = 1 << 0xE,
    }
}