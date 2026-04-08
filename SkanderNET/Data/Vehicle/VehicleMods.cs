using System.Collections.Generic;

namespace SkanderNET.Data.Vehicle
{
    public enum VehicleModType
    {
        Performance,
        Specialty,
        Horn
    }

    public class VehicleMod
    {
        public int Id { get; }
        public string Name { get; }
        internal VehicleMod(int id, string name) { Id = id; Name = name; }
    }

    internal static class VehicleMods
    {
        internal static readonly Dictionary<Toy, Dictionary<VehicleModType, VehicleMod[]>> Mods = new Dictionary<Toy, Dictionary<VehicleModType, VehicleMod[]>>()
        {
            [Toy.ClownCruiser] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Koopa Clown Cap"),
                    new VehicleMod(1, "Banzai Bill Driller"),
                    new VehicleMod(2, "Dry Bone Basher"),
                    new VehicleMod(3, "Royal Figurehead"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Green Shell Plates"),
                    new VehicleMod(1, "Spikey Shell Armor"),
                    new VehicleMod(2, "Steeled Bones"),
                    new VehicleMod(3, "Airship Planks"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Platform Horn"),
                    new VehicleMod(1, "Pip Squeaker Horn"),
                    new VehicleMod(2, "Sky Rage Horn"),
                    new VehicleMod(3, "King's Bellower Horn"),
                }
            },
            [Toy.JetStream] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Wind Slashers"),
                    new VehicleMod(1, "Blade Wings"),
                    new VehicleMod(2, "Air-Flex Flyers"),
                    new VehicleMod(3, "Brassfeather Wings"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Wind Frenzies"),
                    new VehicleMod(1, "Vortex Core Turbine"),
                    new VehicleMod(2, "Vintage Forge Crank"),
                    new VehicleMod(3, "Brasslock Engine"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Turbine Trumpet"),
                    new VehicleMod(1, "Avian Sirens"),
                    new VehicleMod(2, "Air Horn"),
                    new VehicleMod(3, "Mallard Quacker"),
                }
            },
            [Toy.SkySlicer] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Katar Wing"),
                    new VehicleMod(1, "Zero Friction Plate"),
                    new VehicleMod(2, "Double Edge Hull"),
                    new VehicleMod(3, "Sal Wing Shell"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Screamstream Thrust"),
                    new VehicleMod(1, "Uni-Fusion Boost"),
                    new VehicleMod(2, "Twin Jet Turbos"),
                    new VehicleMod(3, "Tri-Jet Threat"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Cloud Reverberator"),
                    new VehicleMod(1, "Radar Resonator"),
                    new VehicleMod(2, "Electro Listener"),
                    new VehicleMod(3, "Sky Call"),
                }
            },
            [Toy.SeaShadow] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Sea Chasm Turbine"),
                    new VehicleMod(1, "Bio-Luminator"),
                    new VehicleMod(2, "Pressure Reactor"),
                    new VehicleMod(3, "Deep Sea Dentures"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Undercurrent Catcher"),
                    new VehicleMod(1, "Kraken Coating"),
                    new VehicleMod(2, "Dark Manta Fins"),
                    new VehicleMod(3, "Alluring Lures"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Night Time Tuner"),
                    new VehicleMod(1, "Abyssal Emitter"),
                    new VehicleMod(2, "Manta Music"),
                    new VehicleMod(3, "Sea Floor Rumbler"),
                }
            },
            [Toy.ThumpTruck] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Concrete Chewer"),
                    new VehicleMod(1, "The Dozer"),
                    new VehicleMod(2, "Rough Truck Grill"),
                    new VehicleMod(3, "Experienced Excavator"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Fusion Mixer"),
                    new VehicleMod(1, "Deep Bucket Bed"),
                    new VehicleMod(2, "Rally Spoiler"),
                    new VehicleMod(3, "The Ol' Ball and Chain"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Mammoth Toot"),
                    new VehicleMod(1, "Wrecking Racket"),
                    new VehicleMod(2, "Builder Boom"),
                    new VehicleMod(3, "Hornfoolery"),
                }
            },
            [Toy.SharkTank] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Geode Grinders"),
                    new VehicleMod(1, "Double-Treaded Tracks"),
                    new VehicleMod(2, "Turbo Slick Belts"),
                    new VehicleMod(3, "Shark Teeth Biters"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Subterrain Glider"),
                    new VehicleMod(1, "Gravel Hopper Boosts"),
                    new VehicleMod(2, "Double-Fin Diver"),
                    new VehicleMod(3, "Terra Fin"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Quarry Clang"),
                    new VehicleMod(1, "Bedrock Bop"),
                    new VehicleMod(2, "Sand Shark Alarm"),
                    new VehicleMod(3, "Terrain Trumpet"),
                }
            },
            [Toy.BurnCycle] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Cracked Core Fender"),
                    new VehicleMod(1, "Burning Bumper"),
                    new VehicleMod(2, "Obsidian Shard Guard"),
                    new VehicleMod(3, "Lava Levelers"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Molten Boosters"),
                    new VehicleMod(1, "Magma Injector"),
                    new VehicleMod(2, "Burn-Up Motor"),
                    new VehicleMod(3, "Inferno Wheel"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Eruption Uproar"),
                    new VehicleMod(1, "Cinderinger"),
                    new VehicleMod(2, "Volcanic Volume"),
                    new VehicleMod(3, "Heat Beep"),
                }
            },
            [Toy.HotStreak] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Blue-Fire Tires"),
                    new VehicleMod(1, "Road Burners"),
                    new VehicleMod(2, "Coal Shooters"),
                    new VehicleMod(3, "Speed Demons"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Blaze Boosters"),
                    new VehicleMod(1, "Volcanic Vents"),
                    new VehicleMod(2, "Block Broiler"),
                    new VehicleMod(3, "Wind Flares"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Searing Snarler"),
                    new VehicleMod(1, "Pop Snapper"),
                    new VehicleMod(2, "Fire Crackle"),
                    new VehicleMod(3, "Semi Blaze Horn"),
                }
            },
            [Toy.BuzzWing] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Monarch Lifters"),
                    new VehicleMod(1, "Forest Patched Wings"),
                    new VehicleMod(2, "Beetle Shell Blast Plates"),
                    new VehicleMod(3, "Bumble Buzzers"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Butterfly Flappers"),
                    new VehicleMod(1, "Dragon Dasher"),
                    new VehicleMod(2, "Beetle Tune-up"),
                    new VehicleMod(3, "The King Bee"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Cricket Crackler"),
                    new VehicleMod(1, "Mantis Clack"),
                    new VehicleMod(2, "Grasshopper Honker"),
                    new VehicleMod(3, "Ant Alarm"),
                }
            },
            [Toy.StealthStinger] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Leaf Cutters"),
                    new VehicleMod(1, "Pine Cone Rotors"),
                    new VehicleMod(2, "Petal Paddles"),
                    new VehicleMod(3, "Lacquered Coaxial"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Waxed Wood Plates"),
                    new VehicleMod(1, "Acorn Armoring"),
                    new VehicleMod(2, "Briar Patching"),
                    new VehicleMod(3, "Hardwood Shielding"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Woodland Tune"),
                    new VehicleMod(1, "Timber Wailer"),
                    new VehicleMod(2, "Bramble Blare"),
                    new VehicleMod(3, "Grove Groover"),
                }
            },
            [Toy.SunRunner] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Star Crystals"),
                    new VehicleMod(1, "Laser Array"),
                    new VehicleMod(2, "Planetold Mover"),
                    new VehicleMod(3, "Meteor Crux"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Shuttle Dom"),
                    new VehicleMod(1, "Refracting Prism"),
                    new VehicleMod(2, "Solar Flare Blocker"),
                    new VehicleMod(3, "Crystalline Cover"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Noisy Nebulizer"),
                    new VehicleMod(1, "Anti-Grav Vibration"),
                    new VehicleMod(2, "The Little Bang"),
                    new VehicleMod(3, "Interface Blips"),
                }
            },
            [Toy.SodaSkimmer] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Gushing Geysers"),
                    new VehicleMod(1, "Bobbing Blast"),
                    new VehicleMod(2, "Twist-off Twisters"),
                    new VehicleMod(3, "Booming Bubbles"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Inner Beast Tube"),
                    new VehicleMod(1, "Pontoon Mixers"),
                    new VehicleMod(2, "Carbonator"),
                    new VehicleMod(3, "Fizzy Floaters"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Pressure Blare"),
                    new VehicleMod(1, "Fizzled Foghorn"),
                    new VehicleMod(2, "Party Pop!"),
                    new VehicleMod(3, "Caffeinated Jitter"),
                }
            },
            [Toy.SplatterSplasher] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Easel Exhausts"),
                    new VehicleMod(1, "Abstract Driller"),
                    new VehicleMod(2, "Modern Hydros"),
                    new VehicleMod(3, "Watercolor Mixer"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Speed Sculpture"),
                    new VehicleMod(1, "Ink Jets"),
                    new VehicleMod(2, "Acrylic Spoiler"),
                    new VehicleMod(3, "Paint Palette Wheel"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Bristle Whistle"),
                    new VehicleMod(1, "Graffiti Clamor"),
                    new VehicleMod(2, "Airbrush Horn"),
                    new VehicleMod(3, "Hue Tone"),
                }
            },
            [Toy.BarrelBlaster] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Barrel Bracer"),
                    new VehicleMod(1, "Steel Drum Driver"),
                    new VehicleMod(2, "Kong Crafted Engine"),
                    new VehicleMod(3, "Simian Street Burners"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Jungle Tree Plates"),
                    new VehicleMod(1, "Baddie Crusher"),
                    new VehicleMod(2, "Banana Coal Catcher"),
                    new VehicleMod(3, "Bonus Stage Boosters"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Tree Top Whistle Horn"),
                    new VehicleMod(1, "Howling Horn"),
                    new VehicleMod(2, "Factory Valve Horn"),
                    new VehicleMod(3, "Old School Sound Horn"),
                }
            },
            [Toy.GoldRusher] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Pop & Sparkers"),
                    new VehicleMod(1, "Canyon Jump Boosts"),
                    new VehicleMod(2, "Show Sparklers"),
                    new VehicleMod(3, "Tall Pipe Flares"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Cogwheel Trike"),
                    new VehicleMod(1, "Gearwork Kit"),
                    new VehicleMod(2, "Coin Cycle*"),
                    new VehicleMod(3, "Short Fuse Quads"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Clink Clankers"),
                    new VehicleMod(1, "Widget Whir"),
                    new VehicleMod(2, "Glided Gizmo"),
                    new VehicleMod(3, "Honkamajig"),
                }
            },
            [Toy.ShieldStriker] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Troll Patroller"),
                    new VehicleMod(1, "Chompy Guard"),
                    new VehicleMod(2, "Full Metal Fender"),
                    new VehicleMod(3, "High Speed Pursuer"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Tactical Viewer"),
                    new VehicleMod(1, "Lightium Lamps"),
                    new VehicleMod(2, "Fitted Tank Top"),
                    new VehicleMod(3, "Lightning Strikers"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Redirectors"),
                    new VehicleMod(1, "Battery Buzz"),
                    new VehicleMod(2, "Shield Sirens"),
                    new VehicleMod(3, "Grinding Gears"),
                }
            },
            [Toy.CryptCrusher] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Coffin Clapper"),
                    new VehicleMod(1, "Wood Geist"),
                    new VehicleMod(2, "Mummy Motif"),
                    new VehicleMod(3, "Fiesta Fueled Engine"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Grave Raiser"),
                    new VehicleMod(1, "Stage Coach Cruiser"),
                    new VehicleMod(2, "Mums Spindle"),
                    new VehicleMod(3, "Percussion Pusher"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Creepy Critter Call Horn"),
                    new VehicleMod(1, "Decomposition Horn"),
                    new VehicleMod(2, "Cacoffiny Horn"),
                    new VehicleMod(3, "Honk O' Ween Horn"),
                }
            },
            [Toy.TombBuggy] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Grave Crackers"),
                    new VehicleMod(1, "Ghastly Speedsters"),
                    new VehicleMod(2, "Rib Rattlers"),
                    new VehicleMod(3, "Bonesaw Rippers"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Boo-ster"),
                    new VehicleMod(1, "Ecto-Engine"),
                    new VehicleMod(2, "Tombstone Smoker"),
                    new VehicleMod(3, "Vampire Ventilation"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Scream Screech"),
                    new VehicleMod(1, "Organ Blast"),
                    new VehicleMod(2, "Underworld Hum"),
                    new VehicleMod(3, "Spectral Spooker"),
                }
            },
            [Toy.DiveBomber] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Torpedo Buoys"),
                    new VehicleMod(1, "Sub Stream Jets"),
                    new VehicleMod(2, "Flex Floaties"),
                    new VehicleMod(3, "Arkeyan Echo Guns"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Power Propeller"),
                    new VehicleMod(1, "Deep Dynamo"),
                    new VehicleMod(2, "Mr. Sqeeks"),
                    new VehicleMod(3, "Aqua Splitter"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Blaring Seahorn"),
                    new VehicleMod(1, "Dolphin Disorientor Horn"),
                    new VehicleMod(2, "Lost at Sea Signal Horn"),
                    new VehicleMod(3, "High Pressure Whistle Horn"),
                }
            },
            [Toy.ReefRipper] = new Dictionary<VehicleModType, VehicleMod[]>()
            {
                [VehicleModType.Performance] = new[]
                {
                    new VehicleMod(0, "Gill Grill"),
                    new VehicleMod(1, "Spearfish Driver"),
                    new VehicleMod(2, "Thumpback Prow"),
                    new VehicleMod(3, "Trident Spearheads"),
                },
                [VehicleModType.Specialty] = new[]
                {
                    new VehicleMod(0, "Aqua Fin Turbine"),
                    new VehicleMod(1, "Sharpfin Blaster"),
                    new VehicleMod(2, "Whale Flipper"),
                    new VehicleMod(3, "Tidal Wave Crasher"),
                },
                [VehicleModType.Horn] = new[]
                {
                    new VehicleMod(0, "Revving Gurgler"),
                    new VehicleMod(1, "High-Tide Honker"),
                    new VehicleMod(2, "Ocean Buzz"),
                    new VehicleMod(3, "Thumpback Call"),
                }
            },
        };
    }
}