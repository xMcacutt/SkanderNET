using System;

namespace SkanderNET
{
    [Flags]
    internal enum Platform2011 : byte
    {
        None = 0,
        Wii = 1 << 0,
        Xbox360 = 1 << 1,
        PlayStation3 = 1 << 2,
        PC = 1 << 3,
        Nintendo3ds = 1 << 4
    }

    [Flags]
    internal enum Platform2013 : byte
    {
        None = 0,
        WiiU = 1 << 0,
        XboxOne = 1 << 1,
        PlayStation4 = 1 << 2,
        iOS = 1 << 3,
        NintendoSwitch = 1 << 6
    }

    [Flags]
    public enum Platform
    {
        None = 0,
        Wii = 1 << 0,
        Xbox360 = 1 << 1,
        PlayStation3 = 1 << 2,
        PC = 1 << 3,
        Nintendo3ds = 1 << 4,
        WiiU = 1 << 5,
        XboxOne = 1 << 6,
        PlayStation4 = 1 << 7,
        iOS = 1 << 8,
        NintendoSwitch = 1 << 9
    }
}