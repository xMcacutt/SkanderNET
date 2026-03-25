using System.Collections.Generic;

namespace SkanderNET
{
    public enum VehicleNeonType : byte
    {
        None = 0x0,
        Darkness = 0x1,
        Eon = 0x2,
        Ancient = 0x3,
        CapNCluck = 0x4,
        Cartoon = 0x5,
        Kaos = 0x6,
        Police = 0x7,
        Construction = 0x8,
        Holiday = 0x9,
        Ghost = 0xA,
        Royal = 0xB,
        Ninja = 0xC,
        Thermal = 0xD,
        Robot = 0xE,
        FireTruck = 0xF,
    }
    
    internal class VehicleNeonIndex
    {
        internal static readonly Dictionary<VehicleNeonType, string> Neons = new Dictionary<VehicleNeonType, string>
        {
            { VehicleNeonType.Darkness, "Darkness" },
            { VehicleNeonType.Eon, "Eon" },
            { VehicleNeonType.Ancient, "Ancient" },
            { VehicleNeonType.CapNCluck, "Cap'N Cluck" },
            { VehicleNeonType.Cartoon, "Cartoon" },
            { VehicleNeonType.Kaos, "Kaos" },
            { VehicleNeonType.Police, "Police" },
            { VehicleNeonType.Construction, "Construction" },
            { VehicleNeonType.Holiday, "Holiday" },
            { VehicleNeonType.Ghost, "Ghost" },
            { VehicleNeonType.Royal, "Royal" },
            { VehicleNeonType.Ninja, "Ninja" },
            { VehicleNeonType.Thermal, "Thermal" },
            { VehicleNeonType.Robot, "Robot" },
            { VehicleNeonType.FireTruck, "Fire Truck" },
        };
    }
}