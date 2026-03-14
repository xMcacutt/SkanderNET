using System;
using System.Collections.Generic;

namespace SkanderNET
{
    public enum SkillPath
    {
        Left,
        Right
    }
    
    internal struct SkillData
    {
        private readonly ushort _flags;

        internal SkillData(ushort flags)
        {
            _flags = flags;
        }

        internal ushort Raw => _flags;
        internal bool PathChosen => (_flags & 0x1) != 0;
        internal bool IsRightPath => (_flags & 0x2) != 0;

        internal bool IsLeftPath => PathChosen && !IsRightPath;

        internal bool HasUpgrade(int index)
        {
            if (index < 0 || index > 15)
                throw new ArgumentOutOfRangeException(nameof(index));
            return (_flags & (1 << index)) != 0;
        }
        
        internal SkillData WithUpgrade(int index)
        {
            if (index < 0 || index > 15)
                throw new ArgumentOutOfRangeException(nameof(index));

            return new SkillData((ushort)(_flags | (1 << index)));
        }
        
        internal SkillData WithoutUpgrade(int index)
        {
            if (index < 0 || index > 15)
                throw new ArgumentOutOfRangeException(nameof(index));

            return new SkillData((ushort)(_flags & ~(1 << index)));
        }
        
        internal SkillData ChooseLeftPath()
        {
            ushort f = (ushort)(_flags | 0x1);
            f &= unchecked((ushort)~0x2);
            return new SkillData(f);
        }

        internal SkillData ChooseRightPath()
        {
            ushort f = (ushort)(_flags | 0x3);
            return new SkillData(f);
        }
    }
}