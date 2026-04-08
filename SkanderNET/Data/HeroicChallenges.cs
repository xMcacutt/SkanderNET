using System;

namespace SkanderNET.Data
{
    [Flags]
    public enum HeroicChallengesSpyrosAdventure : uint
    {
        None = 0,
        ChompyChompDown = 1 << 0x0,
        ThisBombsForYou = 1 << 0x01, 
        JumpForIt = 1 << 0x02, 
        WhereArtThou,Paintings = 1 << 0x03, 
        LairOfTheGiantSpiders = 1 << 0x04, 
        FightTeleportFight = 1 << 0x05, 
        TheThreeTeleporters = 1 << 0x06, 
        StopSheepThieves = 1 << 0x07, 
        MiningForCharms = 1 << 0x08, 
        DungeonessCreeps = 1 << 0x09, 
        MiningIsTheKey = 1 << 0x0A, 
        MissionAchomplished = 1 << 0x0B, 
        PodGauntlet = 1 << 0x0C, 
        TimesAWastin = 1 << 0x0D, 
        SaveThePurpleChompies = 1 << 0x0E, 
        SpawnerCave = 1 << 0x0F, 
        ArachnidAntechamber = 1 << 0x10, 
        HobsonsChoice = 1 << 0x11, 
        IsleOfTheAutomatons = 1 << 0x12, 
        YouBreakItYouBuyIt = 1 << 0x13, 
        MinefieldMishap = 1 << 0x14, 
        LobsOFun = 1 << 0x15, 
        SpellPunked = 1 << 0x16, 
        CharmHunt = 1 << 0x17, 
        FlipTheScript = 1 << 0x18, 
        YouveStolenMyHearts = 1 << 0x19, 
        BombsToTheWalls = 1 << 0x1A, 
        OperationSheepFreedom = 1 << 0x1B, 
        Jailbreak = 1 << 0x1C, 
        EnvironmentallyUnfriendly = 1 << 0x1D, 
        ChemicalCleanup = 1 << 0x1E, 
        BreakTheCats = 1u << 0x1F, 
    }
    
    [Flags]
    public enum HeroicChallengesGiants : uint
    {
        FlamePiratesOnIce = 1 << 0x00,
        SkylandsSalute = 1 << 0x01,
        Sabrina = 1 << 0x02,
        TheSkyIsFalling = 1 << 0x03,
        NortsWinterClassic = 1 << 0x04,
        BreakTheFakes = 1 << 0x05,
        BakingWithBatterson = 1 << 0x06,
        BlobbersFolly = 1 << 0x07,
        DeliveryDay = 1 << 0x0E,
        GiveAHoot = 1 << 0x0F,
        ZombieDanceParty = 1 << 0x10,
        ShepherdsPie = 1 << 0x11,
        WatermelonsEleven = 1 << 0x12,
        ARealGoatGetter = 1 << 0x13,
        WoolyBullies = 1 << 0x14,
        TheGreatPancakeSlalom = 1 << 0x15,
        ShootFirstShootLater = 1 << 0x16,
        TheKingsBreech = 1 << 0x17,
    }
}