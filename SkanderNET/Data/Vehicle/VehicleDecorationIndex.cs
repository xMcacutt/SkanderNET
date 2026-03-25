using System.Collections.Generic;

namespace SkanderNET
{
    public enum VehicleDecorationType : byte
    {
        None = 0x0,
        Darkness = 0x1,
        CapNCluck = 0x2,
        Ancient = 0x3,
        Cartoon = 0x4,
        Eon = 0x5,
        Kaos = 0x6,
        Police = 0x7,
        Construction = 0x8,
        Holiday = 0x9,
        Ghost = 0xA,
        Thermal = 0xB,
        FireTruck = 0xC,
        Ninja = 0xD,
        Royal = 0xE,
        Robot = 0xF
    }
    
    internal class VehicleDecorationIndex
    {
        internal static readonly Dictionary<VehicleDecorationType, string> Decorations = new Dictionary<VehicleDecorationType, string>
        {
            { VehicleDecorationType.Darkness, "None" },
            { VehicleDecorationType.CapNCluck, "Darkness" },
            { VehicleDecorationType.Ancient, "Cap'N Cluck" },
            { VehicleDecorationType.Cartoon, "Ancient" },
            { VehicleDecorationType.Eon, "Cartoon" },
            { VehicleDecorationType.Kaos, "Eon" },
            { VehicleDecorationType.Police, "Kaos" },
            { VehicleDecorationType.Construction, "Police" },
            { VehicleDecorationType.Holiday, "Construction" },
            { VehicleDecorationType.Ghost, "Holiday" },
            { VehicleDecorationType.Thermal, "Ghost" },
            { VehicleDecorationType.FireTruck, "Thermal" },
            { VehicleDecorationType.Ninja, "Fire Truck" },
            { VehicleDecorationType.Royal, "Ninja" },
            { VehicleDecorationType.Robot, "Royal" }
        };
    }
}