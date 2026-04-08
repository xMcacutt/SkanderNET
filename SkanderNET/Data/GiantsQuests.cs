using System;
using SkanderNET.Util;

namespace SkanderNET.Data
{
    public struct GiantsQuests
    {
        private byte[] _data;

        internal GiantsQuests(byte[] data)
        {
            _data = null;
            if (data == null || data.Length != 0x9)
                return;
            _data = (byte[])data.Clone();
        }

        internal byte[] GetData()
        {
            var copy = new byte[0x9];
            Array.Copy(_data, copy, copy.Length);
            return copy;
        }

        internal uint MonsterMasher
        {
            get { return Utils.ReadBits(_data, 0x0, 0xA); }
            set { Utils.WriteBits(_data, 0x0, 0xA, value); }
        }
        
        internal uint BattleChamp
        {
            get { return Utils.ReadBits(_data, 0xA, 0x4); }
            set { Utils.WriteBits(_data, 0xA, 0x4, value); }
        }
        
        internal uint ChowHound
        {
            get { return Utils.ReadBits(_data, 0xE, 0x6); }
            set { Utils.WriteBits(_data, 0xE, 0x6, value); }
        }
        
        internal uint HeroicChallenger
        {
            get { return Utils.ReadBits(_data, 0x14, 0x1); }
            set { Utils.WriteBits(_data, 0x14, 0x1, value); }
        }
        
        internal uint ArenaArtist
        {
            get { return Utils.ReadBits(_data, 0x15, 0x1); }
            set { Utils.WriteBits(_data, 0x15, 0x1, value); }
        }
        
        internal uint Elementalist
        {
            get { return Utils.ReadBits(_data, 0x16, 0xD); }
            set { Utils.WriteBits(_data, 0x16, 0xD, value); }
        }
        
        internal uint Stonesmith
        {
            get { return Utils.ReadBits(_data, 0x23, 0x5); }
            set { Utils.WriteBits(_data, 0x23, 0x5, value); }
        }
        
        internal uint Wrecker
        {
            get { return Utils.ReadBits(_data, 0x28, 0x5); }
            set { Utils.WriteBits(_data, 0x28, 0x5, value); }
        }

        internal uint Extinguisher
        {
            get { return Utils.ReadBits(_data, 0x2D, 0x1); }
            set { Utils.WriteBits(_data, 0x2D, 0x1, value); }
        }

        internal uint Waterfall
        {
            get { return Utils.ReadBits(_data, 0x2E, 0x5); }
            set { Utils.WriteBits(_data, 0x2E, 0x5, value); }
        }

        internal uint SkyLooter
        {
            get { return Utils.ReadBits(_data, 0x33, 0x9); }
            set { Utils.WriteBits(_data, 0x33, 0x9, value); }
        }

        internal uint FromAbove
        {
            get { return Utils.ReadBits(_data, 0x3C, 0x5); }
            set { Utils.WriteBits(_data, 0x3C, 0x5, value); }
        }

        internal uint Bombardier
        {
            get { return Utils.ReadBits(_data, 0x41, 0x5); }
            set { Utils.WriteBits(_data, 0x41, 0x5, value); }
        }

        internal uint Steamer
        {
            get { return Utils.ReadBits(_data, 0x46, 0x1); }
            set { Utils.WriteBits(_data, 0x46, 0x1, value); }
        }

        internal uint FullyStocked
        {
            get { return Utils.ReadBits(_data, 0x47, 0x8); }
            set { Utils.WriteBits(_data, 0x47, 0x8, value); }
        }

        internal uint MelonMaestro
        {
            get { return Utils.ReadBits(_data, 0x4F, 0x8); }
            set { Utils.WriteBits(_data, 0x4F, 0x8, value); }
        }

        internal uint ByAThread
        {
            get { return Utils.ReadBits(_data, 0x57, 0x1); }
            set { Utils.WriteBits(_data, 0x57, 0x1, value); }
        }

        internal uint BossedAround
        {
            get { return Utils.ReadBits(_data, 0x58, 0x2); }
            set { Utils.WriteBits(_data, 0x58, 0x2, value); }
        }

        internal uint PuzzlePower
        {
            get { return Utils.ReadBits(_data, 0x5A, 0x5); }
            set { Utils.WriteBits(_data, 0x5A, 0x5, value); }
        }

        internal uint WarpWomper
        {
            get { return Utils.ReadBits(_data, 0x5F, 0x1); }
            set { Utils.WriteBits(_data, 0x5F, 0x1, value); }
        }

        internal uint MagicIsntMight
        {
            get { return Utils.ReadBits(_data, 0x60, 0x6); }
            set { Utils.WriteBits(_data, 0x60, 0x6, value); }
        }

        internal uint Cracker
        {
            get { return Utils.ReadBits(_data, 0x66, 0x5); }
            set { Utils.WriteBits(_data, 0x66, 0x5, value); }
        }

        internal uint IndividualQuest
        {
            get { return Utils.ReadBits(_data, 0x6B, 0x10); }
            set { Utils.WriteBits(_data, 0x6B, 0x10, value); }
        }
    }
}