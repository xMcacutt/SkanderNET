using System;

namespace SkanderNET
{
    internal struct CyosData
    {
        private byte[] _data;

        internal CyosData(byte[] data)
        {
            _data = null;
            if (data == null || data.Length != 0x45)
                return;
            _data = (byte[])data.Clone();
        }

        internal byte[] GetData()
        {
            var copy = new byte[0x45];
            Array.Copy(_data, copy, copy.Length);
            return copy;
        }

        internal uint PrimaryWeapon
        {
            get { return Utils.ReadBits(_data, 0x0, 0xA); }
            set { Utils.WriteBits(_data, 0x0, 0xA, value); }
        }
        
        internal uint SecondaryWeapon
        {
            get { return Utils.ReadBits(_data, 0xA, 0xA); }
            set { Utils.WriteBits(_data, 0xA, 0xA, value); }
        }
        
        internal uint Backpack
        {
            get { return Utils.ReadBits(_data, 0x14, 0xA); }
            set { Utils.WriteBits(_data, 0x14, 0xA, value); }
        }
        
        internal uint Headgear
        {
            get { return Utils.ReadBits(_data, 0x1E, 0xA); }
            set { Utils.WriteBits(_data, 0x1E, 0xA, value); }
        }
        
        internal uint LegGuards
        {
            get { return Utils.ReadBits(_data, 0x28, 0xA); }
            set { Utils.WriteBits(_data, 0x28, 0xA, value); }
        }
        
        internal uint ArmGuards
        {
            get { return Utils.ReadBits(_data, 0x32, 0xA); }
            set { Utils.WriteBits(_data, 0x32, 0xA, value); }
        }

        internal uint ShoulderGuards
        {
            get { return Utils.ReadBits(_data, 0x3C, 0xA); }
            set { Utils.WriteBits(_data, 0x3C, 0xA, value); }
        }

        internal uint Ears
        {
            get { return Utils.ReadBits(_data, 0x46, 0xA); }
            set { Utils.WriteBits(_data, 0x46, 0xA, value); }
        }

        internal uint LowerBodyScale
        {
            get { return Utils.ReadBits(_data, 0x50, 0x4); }
            set { Utils.WriteBits(_data, 0x50, 0x4, value); }
        }

        internal uint UpperBodyScale
        {
            get { return Utils.ReadBits(_data, 0x54, 0x4); }
            set { Utils.WriteBits(_data, 0x54, 0x4, value); }
        }

        internal uint Height
        {
            get { return Utils.ReadBits(_data, 0x58, 0x4); }
            set { Utils.WriteBits(_data, 0x58, 0x4, value); }
        }

        internal uint MuscleScale
        {
            get { return Utils.ReadBits(_data, 0x5C, 0x4); }
            set { Utils.WriteBits(_data, 0x5C, 0x4, value); }
        }

        internal uint HeadScale
        {
            get { return Utils.ReadBits(_data, 0x60, 0x4); }
            set { Utils.WriteBits(_data, 0x60, 0x4, value); }
        }

        internal uint TailWidth
        {
            get { return Utils.ReadBits(_data, 0x64, 0x4); }
            set { Utils.WriteBits(_data, 0x64, 0x4, value); }
        }

        internal uint Head
        {
            get { return Utils.ReadBits(_data, 0x68, 0xA); }
            set { Utils.WriteBits(_data, 0x68, 0xA, value); }
        }

        internal uint Torso
        {
            get { return Utils.ReadBits(_data, 0x72, 0xA); }
            set { Utils.WriteBits(_data, 0x72, 0xA, value); }
        }

        internal uint Arms
        {
            get { return Utils.ReadBits(_data, 0x7C, 0xA); }
            set { Utils.WriteBits(_data, 0x7C, 0xA, value); }
        }

        internal uint LegsTasset
        {
            get { return Utils.ReadBits(_data, 0x86, 0xA); }
            set { Utils.WriteBits(_data, 0x86, 0xA, value); }
        }

        internal uint Tail
        {
            get { return Utils.ReadBits(_data, 0x90, 0x7); }
            set { Utils.WriteBits(_data, 0x90, 0x7, value); }
        }

        internal uint HeadColor1 
        {
            get { return Utils.ReadBits(_data, 0x97, 0x7); } 
            set { Utils.WriteBits(_data, 0x97, 0x7, value); }
        }
        
        internal uint HeadColor2 
        {
            get { return Utils.ReadBits(_data, 0x9E, 0x7); } 
            set { Utils.WriteBits(_data, 0x9E, 0x7, value); }
        }
        
        internal uint HeadColor3 
        {
            get { return Utils.ReadBits(_data, 0xA5, 0x7); } 
            set { Utils.WriteBits(_data, 0xA5, 0x7, value); }
        }
        
        internal uint HeadColor4 
        {
            get { return Utils.ReadBits(_data, 0xAC, 0x7); } 
            set { Utils.WriteBits(_data, 0xAC, 0x7, value); }
        }
        
        internal uint HeadColor5 
        {
            get { return Utils.ReadBits(_data, 0xB3, 0x7); } 
            set { Utils.WriteBits(_data, 0xB3, 0x7, value); }
        }
        
        internal uint EarColorUnused 
        {
            get { return Utils.ReadBits(_data, 0xBA, 0x7); } 
            set { Utils.WriteBits(_data, 0xBA, 0x7, value); }
        }

        internal uint ArmsColor1 
        {
            get { return Utils.ReadBits(_data, 0xC1, 0x7); } 
            set { Utils.WriteBits(_data, 0xC1, 0x7, value); }
        }
        
        internal uint ArmsColor2 
        {
            get { return Utils.ReadBits(_data, 0xC8, 0x7); } 
            set { Utils.WriteBits(_data, 0xC8, 0x7, value); }
        }
        
        internal uint ArmsColor3 
        {
            get { return Utils.ReadBits(_data, 0xCF, 0x7); } 
            set { Utils.WriteBits(_data, 0xCF, 0x7, value); }
        }
        
        internal uint ArmsColor4 
        {
            get { return Utils.ReadBits(_data, 0xD6, 0x7); } 
            set { Utils.WriteBits(_data, 0xD6, 0x7, value); }
        }
        
        internal uint ArmsColor5 
        {
            get { return Utils.ReadBits(_data, 0xDD, 0x7); } 
            set { Utils.WriteBits(_data, 0xDD, 0x7, value); }
        }

        internal uint TorsoColor1 
        {
            get { return Utils.ReadBits(_data, 0xE4, 0x7); } 
            set { Utils.WriteBits(_data, 0xE4, 0x7, value); }
        }
        
        internal uint TorsoColor2 
        {
            get { return Utils.ReadBits(_data, 0xEB, 0x7); } 
            set { Utils.WriteBits(_data, 0xEB, 0x7, value); }
        }
        
        internal uint TorsoColor3 
        {
            get { return Utils.ReadBits(_data, 0xF2, 0x7); } 
            set { Utils.WriteBits(_data, 0xF2, 0x7, value); }
        }
        
        internal uint TorsoColor4 
        {
            get { return Utils.ReadBits(_data, 0xF9, 0x7); } 
            set { Utils.WriteBits(_data, 0xF9, 0x7, value); }
        }
        
        internal uint TorsoColor5 
        {
            get { return Utils.ReadBits(_data, 0x100, 0x7); } 
            set { Utils.WriteBits(_data, 0x100, 0x7, value); }
        }

        internal uint LegsTassetColor1 
        {
            get { return Utils.ReadBits(_data, 0x107, 0x7); } 
            set { Utils.WriteBits(_data, 0x107, 0x7, value); }
        }
        
        internal uint LegsTassetColor2 
        {
            get { return Utils.ReadBits(_data, 0x10E, 0x7); } 
            set { Utils.WriteBits(_data, 0x10E, 0x7, value); }
        }
        
        internal uint LegsTassetColor3 
        {
            get { return Utils.ReadBits(_data, 0x115, 0x7); } 
            set { Utils.WriteBits(_data, 0x115, 0x7, value); }
        }
        
        internal uint LegsTassetColor4 
        {
            get { return Utils.ReadBits(_data, 0x11C, 0x7); } 
            set { Utils.WriteBits(_data, 0x11C, 0x7, value); }
        }
        
        internal uint LegsTassetColor5 
        {
            get { return Utils.ReadBits(_data, 0x123, 0x7); } 
            set { Utils.WriteBits(_data, 0x123, 0x7, value); }
        }

        internal uint EyeColor1 
        {
            get { return Utils.ReadBits(_data, 0x12A, 0x7); } 
            set { Utils.WriteBits(_data, 0x12A, 0x7, value); }
        }
        
        internal uint EyeColor2 
        {
            get { return Utils.ReadBits(_data, 0x131, 0x7); } 
            set { Utils.WriteBits(_data, 0x131, 0x7, value); }
        }

        internal uint TailColor1 
        {
            get { return Utils.ReadBits(_data, 0x138, 0x7); } 
            set { Utils.WriteBits(_data, 0x138, 0x7, value); }
        }
        
        internal uint TailColor2 
        {
            get { return Utils.ReadBits(_data, 0x13F, 0x7); } 
            set { Utils.WriteBits(_data, 0x13F, 0x7, value); }
        }

        internal uint EarsColor1 
        {
            get { return Utils.ReadBits(_data, 0x146, 0x7); } 
            set { Utils.WriteBits(_data, 0x146, 0x7, value); }
        }
        
        internal uint EarsColor2 
        {
            get { return Utils.ReadBits(_data, 0x14D, 0x7); } 
            set { Utils.WriteBits(_data, 0x14D, 0x7, value); }
        }
        
        internal uint EarsColor3 
        {
            get { return Utils.ReadBits(_data, 0x154, 0x7); } 
            set { Utils.WriteBits(_data, 0x154, 0x7, value); }
        }

        internal uint HeadgearColor1 
        {
            get { return Utils.ReadBits(_data, 0x15B, 0x7); } 
            set { Utils.WriteBits(_data, 0x15B, 0x7, value); }
        }
        
        internal uint HeadgearColor2 
        {
            get { return Utils.ReadBits(_data, 0x162, 0x7); } 
            set { Utils.WriteBits(_data, 0x162, 0x7, value); }
        }
        
        internal uint HeadgearColor3 
        {
            get { return Utils.ReadBits(_data, 0x169, 0x7); } 
            set { Utils.WriteBits(_data, 0x169, 0x7, value); }
        }

        internal uint ArmGuardsColor1 
        {
            get { return Utils.ReadBits(_data, 0x170, 0x7); } 
            set { Utils.WriteBits(_data, 0x170, 0x7, value); }
        }
        
        internal uint ArmGuardsColor2 
        {
            get { return Utils.ReadBits(_data, 0x177, 0x7); } 
            set { Utils.WriteBits(_data, 0x177, 0x7, value); }
        }
        
        internal uint ArmGuardsColor3 
        {
            get { return Utils.ReadBits(_data, 0x17E, 0x7); } 
            set { Utils.WriteBits(_data, 0x17E, 0x7, value); }
        }

        internal uint ShoulderGuardsColor1 
        {
            get { return Utils.ReadBits(_data, 0x185, 0x7); } 
            set { Utils.WriteBits(_data, 0x185, 0x7, value); }
        }
        
        internal uint ShoulderGuardsColor2 
        {
            get { return Utils.ReadBits(_data, 0x18C, 0x7); } 
            set { Utils.WriteBits(_data, 0x18C, 0x7, value); }
        }
        
        internal uint ShoulderGuardsColor3 
        {
            get { return Utils.ReadBits(_data, 0x193, 0x7); } 
            set { Utils.WriteBits(_data, 0x193, 0x7, value); }
        }

        internal uint BackpackColor1 
        {
            get { return Utils.ReadBits(_data, 0x19A, 0x7); } 
            set { Utils.WriteBits(_data, 0x19A, 0x7, value); }
        }
        
        internal uint BackpackColor2 
        {
            get { return Utils.ReadBits(_data, 0x1A1, 0x7); } 
            set { Utils.WriteBits(_data, 0x1A1, 0x7, value); }
        }
        internal uint BackpackColor3 
        {
            get { return Utils.ReadBits(_data, 0x1A8, 0x7); } 
            set { Utils.WriteBits(_data, 0x1A8, 0x7, value); }
        }

        internal uint LegGuardsColor1 
        {
            get { return Utils.ReadBits(_data, 0x1AF, 0x7); } 
            set { Utils.WriteBits(_data, 0x1AF, 0x7, value); }
        }
        
        internal uint LegGuardsColor2 
        {
            get { return Utils.ReadBits(_data, 0x1B6, 0x7); } 
            set { Utils.WriteBits(_data, 0x1B6, 0x7, value); }
        }
        
        internal uint LegGuardsColor3 
        {
            get { return Utils.ReadBits(_data, 0x1BD, 0x7); } 
            set { Utils.WriteBits(_data, 0x1BD, 0x7, value); }
        }

        internal uint SecondPowerFlags
        {
            get { return Utils.ReadBits(_data, 0x1C4, 0xA); }
            set { Utils.WriteBits(_data, 0x1C4, 0xA, value); }
        }

        internal uint TertiaryPowerFlags
        {
            get { return Utils.ReadBits(_data, 0x1CE, 0xA); }
            set { Utils.WriteBits(_data, 0x1CE, 0xA, value); }
        }

        internal uint BattleClass
        {
            get { return Utils.ReadBits(_data, 0x1D8, 0x7); }
            set { Utils.WriteBits(_data, 0x1D8, 0x7, value); }
        }

        internal uint Aura
        {
            get { return Utils.ReadBits(_data, 0x1DF, 0x6); }
            set { Utils.WriteBits(_data, 0x1DF, 0x6, value); }
        }

        internal uint SoundEffects
        {
            get { return Utils.ReadBits(_data, 0x1E5, 0x8); }
            set { Utils.WriteBits(_data, 0x1E5, 0x8, value); }
        }

        internal uint Eyes
        {
            get { return Utils.ReadBits(_data, 0x1ED, 0x6); }
            set { Utils.WriteBits(_data, 0x1ED, 0x6, value); }
        }

        internal uint Catchphrase1
        {
            get { return Utils.ReadBits(_data, 0x1F3, 0x8); }
            set { Utils.WriteBits(_data, 0x1F3, 0x8, value); }
        }

        internal uint Catchphrase2
        {
            get { return Utils.ReadBits(_data, 0x1FB, 0x8); }
            set { Utils.WriteBits(_data, 0x1FB, 0x8, value); }
        }

        internal uint Music
        {
            get { return Utils.ReadBits(_data, 0x203, 0x8); }
            set { Utils.WriteBits(_data, 0x203, 0x8, value); }
        }

        internal uint Voice
        {
            get { return Utils.ReadBits(_data, 0x20B, 0x8); }
            set { Utils.WriteBits(_data, 0x20B, 0x8, value); }
        }

        internal uint VoiceFilter
        {
            get { return Utils.ReadBits(_data, 0x213, 0x7); }
            set { Utils.WriteBits(_data, 0x213, 0x7, value); }
        }

        internal uint PrimaryPowerFlags
        {
            get { return Utils.ReadBits(_data, 0x21A, 0x9); }
            set { Utils.WriteBits(_data, 0x21A, 0x9, value); }
        }
    }
}