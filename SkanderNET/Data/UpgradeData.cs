using System;
using System.Collections.Generic;

namespace SkanderNET
{
    public enum UpgradePath
    {
        Left,
        Right,
        None
    }
    
    public struct UpgradeData
    {
        private readonly ushort _flags;

        internal UpgradeData(ushort flags)
        {
            _flags = flags;
        }

        internal ushort Raw => _flags;
        internal bool HasChosenPath => (_flags & 0x1) != 0;
        internal UpgradePath Path => !HasChosenPath ? UpgradePath.None : (_flags & 0x2) != 0 ? UpgradePath.Right : UpgradePath.Left;
        internal bool HasSoulGem => HasUpgrade(9);
        internal bool HasWowPow => HasUpgrade(10);

        internal bool HasUpgrade(int index)
        {
            if (index < 0 || index > 13)
                throw new ArgumentOutOfRangeException(nameof(index));
            return (_flags & (1 << index)) != 0;
        }
        
        internal UpgradeData WithUpgrade(int index)
        {
            if (index < 0 || index > 15)
                throw new ArgumentOutOfRangeException(nameof(index));

            return new UpgradeData((ushort)(_flags | (1 << index)));
        }
        
        internal UpgradeData WithoutUpgrade(int index)
        {
            if (index < 0 || index > 15)
                throw new ArgumentOutOfRangeException(nameof(index));

            return new UpgradeData((ushort)(_flags & ~(1 << index)));
        }
        
        internal UpgradeData ChooseLeftPath()
        {
            if (Path == UpgradePath.Left)
                return this;
            var f = _flags;
            if (Path == UpgradePath.Right)
                f = SwapBits(f, 6, 11);
            f |= 0x1;
            f &= unchecked((ushort)~0x2);
            return new UpgradeData(f);
        }

        internal UpgradeData ChooseRightPath()
        {
            if (Path == UpgradePath.Right)
                return this;
            var f = _flags;
            if (Path == UpgradePath.Left)
                f = SwapBits(f, 6, 11);
            f |= 0x3;
            return new UpgradeData(f);
        }
        
        internal UpgradeData ClearPathUpgrades()
        {
            var f = _flags;
            f &= unchecked((ushort)~0x3);
            f &= unchecked((ushort)~((0x7 << 6) | (0x7 << 11)));
            return new UpgradeData(f);
        }

        private static ushort SwapBits(ushort value, int aStart, int bStart)
        {
            var a = (ushort)((value >> aStart) & 0x7);
            var b = (ushort)((value >> bStart) & 0x7);
            value &= unchecked((ushort)~(0x7 << aStart));
            value &= unchecked((ushort)~(0x7 << bStart));
            value |= (ushort)(a << bStart);
            value |= (ushort)(b << aStart);
            return value;
        }
    }
}