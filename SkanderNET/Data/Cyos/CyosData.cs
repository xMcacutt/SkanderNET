using System;
using SkanderNET.Cyos;
using SkanderNET.Data.Cyos;

namespace SkanderNET
{
    public struct CyosData
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

        internal CyosWeapon PrimaryWeapon
        {
            get { return (CyosWeapon)Utils.ReadBits(_data, 0x0, 0xA); }
            set { Utils.WriteBits(_data, 0x0, 0xA, (uint)value); }
        }
        
        internal CyosWeapon SecondaryWeapon
        {
            get { return (CyosWeapon)Utils.ReadBits(_data, 0xA, 0xA); }
            set { Utils.WriteBits(_data, 0xA, 0xA, (uint)value); }
        }
        
        internal CyosBackpack Backpack
        {
            get { return (CyosBackpack)Utils.ReadBits(_data, 0x14, 0xA); }
            set { Utils.WriteBits(_data, 0x14, 0xA, (uint)value); }
        }
        
        internal CyosHeadgear Headgear
        {
            get { return (CyosHeadgear)Utils.ReadBits(_data, 0x1E, 0xA); }
            set { Utils.WriteBits(_data, 0x1E, 0xA, (uint)value); }
        }
        
        internal CyosLegGuards LegGuards
        {
            get { return (CyosLegGuards)Utils.ReadBits(_data, 0x28, 0xA); }
            set { Utils.WriteBits(_data, 0x28, 0xA, (uint)value); }
        }
        
        internal CyosArmGuards ArmGuards
        {
            get { return (CyosArmGuards)Utils.ReadBits(_data, 0x32, 0xA); }
            set { Utils.WriteBits(_data, 0x32, 0xA, (uint)value); }
        }

        internal CyosShoulderGuards ShoulderGuards
        {
            get { return (CyosShoulderGuards)Utils.ReadBits(_data, 0x3C, 0xA); }
            set { Utils.WriteBits(_data, 0x3C, 0xA, (uint)value); }
        }

        internal CyosEars Ears
        {
            get { return (CyosEars)Utils.ReadBits(_data, 0x46, 0xA); }
            set { Utils.WriteBits(_data, 0x46, 0xA, (uint)value); }
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

        internal CyosHead Head
        {
            get { return (CyosHead)Utils.ReadBits(_data, 0x68, 0xA); }
            set { Utils.WriteBits(_data, 0x68, 0xA, (uint)value); }
        }

        internal CyosChest Torso
        {
            get { return (CyosChest)Utils.ReadBits(_data, 0x72, 0xA); }
            set { Utils.WriteBits(_data, 0x72, 0xA, (uint)value); }
        }

        internal CyosArms Arms
        {
            get { return (CyosArms)Utils.ReadBits(_data, 0x7C, 0xA); }
            set { Utils.WriteBits(_data, 0x7C, 0xA, (uint)value); }
        }

        internal CyosLegs Legs
        {
            get
            {
                if (BattleClass == CyosBattleClass.Swashbuckler)
                    return CyosLegs.None;
                return (CyosLegs)Utils.ReadBits(_data, 0x86, 0xA);
            }
            set
            {
                if (BattleClass == CyosBattleClass.Swashbuckler)
                    return;
                Utils.WriteBits(_data, 0x86, 0xA, (uint)value);
            }
        }

        internal CyosTasset Tasset
        {
            get
            {
                if (BattleClass != CyosBattleClass.Swashbuckler)
                    return CyosTasset.None;
                return (CyosTasset)Utils.ReadBits(_data, 0x86, 0xA);
            }
            set
            {
                if (BattleClass != CyosBattleClass.Swashbuckler)
                    return;
                Utils.WriteBits(_data, 0x86, 0xA, (uint)value);
            }
        }

        internal CyosTail Tail
        {
            get
            {
                if (BattleClass == CyosBattleClass.Swashbuckler)
                    return CyosTail.None;
                return (CyosTail)Utils.ReadBits(_data, 0x90, 0x7);
            }
            set { Utils.WriteBits(_data, 0x90, 0x7, (uint)value); }
        }
        
        internal CyosSwashbucklerTail SwashBucklerTail
        {
            get
            {
                if (BattleClass != CyosBattleClass.Swashbuckler)
                    return CyosSwashbucklerTail.None;
                return (CyosSwashbucklerTail)Utils.ReadBits(_data, 0x90, 0x7);
            }
            set { Utils.WriteBits(_data, 0x90, 0x7, (uint)value); }
        }

        internal CyosColor HeadColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x97, 0x7); } 
            set { Utils.WriteBits(_data, 0x97, 0x7, (uint)value); }
        }
        
        internal CyosColor HeadColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x9E, 0x7); } 
            set { Utils.WriteBits(_data, 0x9E, 0x7, (uint)value); }
        }
        
        internal CyosColor HeadColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xA5, 0x7); } 
            set { Utils.WriteBits(_data, 0xA5, 0x7, (uint)value); }
        }
        
        internal CyosColor HeadColor4 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xAC, 0x7); } 
            set { Utils.WriteBits(_data, 0xAC, 0x7, (uint)value); }
        }
        
        internal CyosColor HeadColor5 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xB3, 0x7); } 
            set { Utils.WriteBits(_data, 0xB3, 0x7, (uint)value); }
        }
        
        internal CyosColor EarColorUnused 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xBA, 0x7); } 
            set { Utils.WriteBits(_data, 0xBA, 0x7, (uint)value); }
        }

        internal CyosColor ArmsColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xC1, 0x7); } 
            set { Utils.WriteBits(_data, 0xC1, 0x7, (uint)value); }
        }
        
        internal CyosColor ArmsColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xC8, 0x7); } 
            set { Utils.WriteBits(_data, 0xC8, 0x7, (uint)value); }
        }
        
        internal CyosColor ArmsColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xCF, 0x7); } 
            set { Utils.WriteBits(_data, 0xCF, 0x7, (uint)value); }
        }
        
        internal CyosColor ArmsColor4 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xD6, 0x7); } 
            set { Utils.WriteBits(_data, 0xD6, 0x7, (uint)value); }
        }
        
        internal CyosColor ArmsColor5 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xDD, 0x7); } 
            set { Utils.WriteBits(_data, 0xDD, 0x7, (uint)value); }
        }

        internal CyosColor TorsoColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xE4, 0x7); } 
            set { Utils.WriteBits(_data, 0xE4, 0x7, (uint)value); }
        }
        
        internal CyosColor TorsoColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xEB, 0x7); } 
            set { Utils.WriteBits(_data, 0xEB, 0x7, (uint)value); }
        }
        
        internal CyosColor TorsoColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xF2, 0x7); } 
            set { Utils.WriteBits(_data, 0xF2, 0x7, (uint)value); }
        }
        
        internal CyosColor TorsoColor4 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0xF9, 0x7); } 
            set { Utils.WriteBits(_data, 0xF9, 0x7, (uint)value); }
        }
        
        internal CyosColor TorsoColor5 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x100, 0x7); } 
            set { Utils.WriteBits(_data, 0x100, 0x7, (uint)value); }
        }

        internal CyosColor LegsTassetColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x107, 0x7); } 
            set { Utils.WriteBits(_data, 0x107, 0x7, (uint)value); }
        }
        
        internal CyosColor LegsTassetColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x10E, 0x7); } 
            set { Utils.WriteBits(_data, 0x10E, 0x7, (uint)value); }
        }
        
        internal CyosColor LegsTassetColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x115, 0x7); } 
            set { Utils.WriteBits(_data, 0x115, 0x7, (uint)value); }
        }
        
        internal CyosColor LegsTassetColor4 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x11C, 0x7); } 
            set { Utils.WriteBits(_data, 0x11C, 0x7, (uint)value); }
        }
        
        internal CyosColor LegsTassetColor5 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x123, 0x7); } 
            set { Utils.WriteBits(_data, 0x123, 0x7, (uint)value); }
        }

        internal CyosColor EyeColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x12A, 0x7); } 
            set { Utils.WriteBits(_data, 0x12A, 0x7, (uint)value); }
        }
        
        internal CyosColor EyeColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x131, 0x7); } 
            set { Utils.WriteBits(_data, 0x131, 0x7, (uint)value); }
        }

        internal CyosColor TailColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x138, 0x7); } 
            set { Utils.WriteBits(_data, 0x138, 0x7, (uint)value); }
        }
        
        internal CyosColor TailColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x13F, 0x7); } 
            set { Utils.WriteBits(_data, 0x13F, 0x7, (uint)value); }
        }

        internal CyosColor EarsColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x146, 0x7); } 
            set { Utils.WriteBits(_data, 0x146, 0x7, (uint)value); }
        }
        
        internal CyosColor EarsColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x14D, 0x7); } 
            set { Utils.WriteBits(_data, 0x14D, 0x7, (uint)value); }
        }
        
        internal CyosColor EarsColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x154, 0x7); } 
            set { Utils.WriteBits(_data, 0x154, 0x7, (uint)value); }
        }

        internal CyosColor HeadgearColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x15B, 0x7); } 
            set { Utils.WriteBits(_data, 0x15B, 0x7, (uint)value); }
        }
        
        internal CyosColor HeadgearColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x162, 0x7); } 
            set { Utils.WriteBits(_data, 0x162, 0x7, (uint)value); }
        }
        
        internal CyosColor HeadgearColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x169, 0x7); } 
            set { Utils.WriteBits(_data, 0x169, 0x7, (uint)value); }
        }

        internal CyosColor ArmGuardsColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x170, 0x7); } 
            set { Utils.WriteBits(_data, 0x170, 0x7, (uint)value); }
        }
        
        internal CyosColor ArmGuardsColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x177, 0x7); } 
            set { Utils.WriteBits(_data, 0x177, 0x7, (uint)value); }
        }
        
        internal CyosColor ArmGuardsColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x17E, 0x7); } 
            set { Utils.WriteBits(_data, 0x17E, 0x7, (uint)value); }
        }

        internal CyosColor ShoulderGuardsColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x185, 0x7); } 
            set { Utils.WriteBits(_data, 0x185, 0x7, (uint)value); }
        }
        
        internal CyosColor ShoulderGuardsColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x18C, 0x7); } 
            set { Utils.WriteBits(_data, 0x18C, 0x7, (uint)value); }
        }
        
        internal CyosColor ShoulderGuardsColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x193, 0x7); } 
            set { Utils.WriteBits(_data, 0x193, 0x7, (uint)value); }
        }

        internal CyosColor BackpackColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x19A, 0x7); } 
            set { Utils.WriteBits(_data, 0x19A, 0x7, (uint)value); }
        }
        
        internal CyosColor BackpackColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x1A1, 0x7); } 
            set { Utils.WriteBits(_data, 0x1A1, 0x7, (uint)value); }
        }
        internal CyosColor BackpackColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x1A8, 0x7); } 
            set { Utils.WriteBits(_data, 0x1A8, 0x7, (uint)value); }
        }

        internal CyosColor LegGuardsColor1 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x1AF, 0x7); } 
            set { Utils.WriteBits(_data, 0x1AF, 0x7, (uint)value); }
        }
        
        internal CyosColor LegGuardsColor2 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x1B6, 0x7); } 
            set { Utils.WriteBits(_data, 0x1B6, 0x7, (uint)value); }
        }
        
        internal CyosColor LegGuardsColor3 
        {
            get { return (CyosColor)Utils.ReadBits(_data, 0x1BD, 0x7); } 
            set { Utils.WriteBits(_data, 0x1BD, 0x7, (uint)value); }
        }

        internal CyosElementalPower ElementalPower
        {
            get { return (CyosElementalPower)Utils.ReadBits(_data, 0x1C4, 0xA); }
            set { Utils.WriteBits(_data, 0x1C4, 0xA, (uint)value); }
        }

        internal CyosSecretTechnique SecretTechnique
        {
            get { return (CyosSecretTechnique)Utils.ReadBits(_data, 0x1CE, 0xA); }
            set { Utils.WriteBits(_data, 0x1CE, 0xA, (uint)value); }
        }

        internal CyosBattleClass BattleClass
        {
            get { return (CyosBattleClass)Utils.ReadBits(_data, 0x1D8, 0x7); }
            set { Utils.WriteBits(_data, 0x1D8, 0x7, (uint)value); }
        }

        internal CyosAura Aura
        {
            get { return (CyosAura)Utils.ReadBits(_data, 0x1DF, 0x6); }
            set { Utils.WriteBits(_data, 0x1DF, 0x6, (uint)value); }
        }

        internal CyosSoundEffects SoundEffects
        {
            get { return (CyosSoundEffects)Utils.ReadBits(_data, 0x1E5, 0x8); }
            set { Utils.WriteBits(_data, 0x1E5, 0x8, (uint)value); }
        }

        internal CyosEyes Eyes
        {
            get { return (CyosEyes)Utils.ReadBits(_data, 0x1ED, 0x6); }
            set { Utils.WriteBits(_data, 0x1ED, 0x6, (uint)value); }
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

        internal CyosMusic Music
        {
            get { return (CyosMusic)Utils.ReadBits(_data, 0x203, 0x8); }
            set { Utils.WriteBits(_data, 0x203, 0x8, (uint)value); }
        }

        internal CyosVoice Voice
        {
            get { return (CyosVoice)Utils.ReadBits(_data, 0x20B, 0x8); }
            set { Utils.WriteBits(_data, 0x20B, 0x8, (uint)value); }
        }

        internal CyosVoiceFilter VoiceFilter
        {
            get { return (CyosVoiceFilter)Utils.ReadBits(_data, 0x213, 0x7); }
            set { Utils.WriteBits(_data, 0x213, 0x7, (uint)value); }
        }

        internal CyosWeaponPower WeaponPower
        {
            get { return (CyosWeaponPower)Utils.ReadBits(_data, 0x21A, 0x9); }
            set { Utils.WriteBits(_data, 0x21A, 0x9, (uint)value); }
        }
        
        public override string ToString()
        {
            return
                "BattleClass: " + BattleClass + "\n" +
                "Head: " + Head + "\n" +
                "Eyes: " + Eyes + "\n" +
                "Ears: " + Ears + "\n" +
                "Torso: " + Torso + "\n" +
                "Arms: " + Arms + "\n" +
                "Legs: " + Legs + "\n" +
                "Tasset: " + Tasset + "\n" +
                "Tail: " + Tail + "\n" +
                "Headgear: " + Headgear + "\n" +
                "ShoulderGuards: " + ShoulderGuards + "\n" +
                "ArmGuards: " + ArmGuards + "\n" +
                "LegGuards: " + LegGuards + "\n" +
                "Backpack: " + Backpack + "\n" +
                "PrimaryWeapon: " + PrimaryWeapon + "\n" +
                "SecondaryWeapon: " + SecondaryWeapon + "\n" +
                "Aura: " + Aura + "\n" +
                "Height: " + Height + "\n" +
                "UpperBodyScale: " + UpperBodyScale + "\n" +
                "LowerBodyScale: " + LowerBodyScale + "\n" +
                "HeadScale: " + HeadScale + "\n" +
                "MuscleScale: " + MuscleScale + "\n" +
                "TailWidth: " + TailWidth + "\n" +
                "Catchphrase1: " + Catchphrase1 + "\n" +
                "Catchphrase2: " + Catchphrase2 + "\n" +
                "Music: " + Music + "\n" +
                "SoundEffects: " + SoundEffects + "\n" +
                "Voice: " + Voice + "\n" +
                "VoiceFilter: " + VoiceFilter + "\n" +
                "WeaponPower: " + WeaponPower + "\n" +
                "ElementalPower: " + ElementalPower + "\n" +
                "SecretTechnique: " + SecretTechnique;
        }

    }
}