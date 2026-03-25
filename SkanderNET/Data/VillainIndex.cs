using System.Collections.Generic;

namespace SkanderNET
{
    public enum VillainType : ushort
    {
        None = 0,
        ChompyMage = 1,
        Krankcase = 2,
        Wolfgang = 3,
        PepperJack = 4,
        Thief = 5,
        Luminous = 6,
        GoldenQueen = 7,
        DreamTwin = 8,
        Gulper = 9,
        Kaos = 10,
        CuckooClocker = 11,
        BuzzerBeak = 12,
        ShieldShredder = 13,
        CrossCrow = 14,
        BoneChompy = 15,
        BrawlAndChain = 16,
        BombShell = 17,
        MaskerMind = 18,
        ChillBill = 19,
        SheepCreep = 20,
        Shrednaught = 21,
        ChompChest = 22,
        BroccoliGuy = 23,
        RageMage = 24,
        LobGoblin = 25,
        Chompy = 26,
        Fisticuffs = 27,
        TrollingThunder = 28,
        HoodSickle = 29,
        BruiserCruiser = 30,
        Brawlrus = 31,
        TussleSprout = 32,
        Krankenstein = 33,
        ScrapShooter = 34,
        SlobberTrap = 35,
        Grinnade = 36,
        BadJuju = 37,
        BlasterTron = 38,
        TaeKwonCrow = 39,
        PainYatta = 40,
        SmokeScream = 41,
        EyeFive = 42,
        GraveClobber = 43,
        Threatpack = 44,
        MabLobs = 45,
        EyeScream = 46,
    }
    
    internal class VillainMetaData
    {
        public readonly string Name;
        public readonly Element Element;
        
        internal VillainMetaData(string name, Element element = Element.None)
        {
            Name = name;
            Element = element;
        }
    }
    
    internal static class VillainIndex
    {
        internal static readonly Dictionary<VillainType, VillainMetaData> Villains = new Dictionary<VillainType, VillainMetaData>
        {
            { VillainType.ChompyMage, new VillainMetaData("Chompy Mage", Element.Life) },
            { VillainType.Krankcase, new VillainMetaData("Dr. Krankcase", Element.Tech) },
            { VillainType.Wolfgang, new VillainMetaData("Wolfgang", Element.Undead) },
            { VillainType.PepperJack, new VillainMetaData("Chef Pepper Jack", Element.Fire) },
            { VillainType.Thief, new VillainMetaData("Nightshade", Element.Dark) },
            { VillainType.Luminous, new VillainMetaData("Luminous", Element.Light) },
            { VillainType.GoldenQueen, new VillainMetaData("Golden Queen", Element.Earth) },
            { VillainType.DreamTwin, new VillainMetaData("Dreamcatcher", Element.Air) },
            { VillainType.Gulper, new VillainMetaData("The Gulper", Element.Water) },
            { VillainType.Kaos, new VillainMetaData("Kaos", Element.Kaos) },
            { VillainType.CuckooClocker, new VillainMetaData("Cuckoo Clocker", Element.Life) },
            { VillainType.BuzzerBeak, new VillainMetaData("Buzzer Beak", Element.Air) },
            { VillainType.ShieldShredder, new VillainMetaData("Shield Shredder", Element.Life) },
            { VillainType.CrossCrow, new VillainMetaData("Cross Crow", Element.Water) },
            { VillainType.BoneChompy, new VillainMetaData("Bone Chompy", Element.Undead) },
            { VillainType.BrawlAndChain, new VillainMetaData("Brawl & Chain", Element.Water) },
            { VillainType.BombShell, new VillainMetaData("Bomb Shell", Element.Magic) },
            { VillainType.MaskerMind, new VillainMetaData("Masker Mind", Element.Undead) },
            { VillainType.ChillBill, new VillainMetaData("Chill Bill", Element.Water) },
            { VillainType.SheepCreep, new VillainMetaData("Sheep Creep", Element.Life) },
            { VillainType.Shrednaught, new VillainMetaData("Shrednaught", Element.Tech) },
            { VillainType.ChompChest, new VillainMetaData("Chomp Chest", Element.Earth) },
            { VillainType.BroccoliGuy, new VillainMetaData("Broccoli Guy", Element.Life) },
            { VillainType.RageMage, new VillainMetaData("Rage Mage", Element.Magic) },
            { VillainType.LobGoblin, new VillainMetaData("Lob Goblin", Element.Light) },
            { VillainType.Chompy, new VillainMetaData("Chompy", Element.Life) },
            { VillainType.Fisticuffs, new VillainMetaData("Fisticuffs", Element.Dark) },
            { VillainType.TrollingThunder, new VillainMetaData("Trolling Thunder", Element.Tech) },
            { VillainType.HoodSickle, new VillainMetaData("Hood Sickle", Element.Undead) },
            { VillainType.BruiserCruiser, new VillainMetaData("Bruiser Cruiser", Element.Tech) },
            { VillainType.Brawlrus, new VillainMetaData("Brawlrus", Element.Tech) },
            { VillainType.TussleSprout, new VillainMetaData("Tussle Sprout", Element.Earth) },
            { VillainType.Krankenstein, new VillainMetaData("Krankenstein", Element.Air) },
            { VillainType.ScrapShooter, new VillainMetaData("Scrap Shooter", Element.Fire) },
            { VillainType.SlobberTrap, new VillainMetaData("Slobber Trap", Element.Water) },
            { VillainType.Grinnade, new VillainMetaData("Grinnade", Element.Fire) },
            { VillainType.BadJuju, new VillainMetaData("Bad Juju", Element.Air) },
            { VillainType.BlasterTron, new VillainMetaData("Blaster-Tron", Element.Light) },
            { VillainType.TaeKwonCrow, new VillainMetaData("Eye Scream", Element.Dark) },
            { VillainType.PainYatta, new VillainMetaData("Pain-Yatta", Element.Magic) },
            { VillainType.SmokeScream, new VillainMetaData("Smoke Scream", Element.Fire) },
            { VillainType.EyeFive, new VillainMetaData("Eye Five", Element.Light) },
            { VillainType.GraveClobber, new VillainMetaData("Grave Clobber", Element.Earth) },
            { VillainType.Threatpack, new VillainMetaData("Threatpack", Element.Water) },
            { VillainType.MabLobs, new VillainMetaData("Mab Lobs", Element.Tech) },
            { VillainType.EyeScream, new VillainMetaData("Eye Scream", Element.Dark) }
        };
    }
}