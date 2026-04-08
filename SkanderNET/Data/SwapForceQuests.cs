using System;
using SkanderNET.Util;

namespace SkanderNET.Data
{
    public struct SwapForceQuests
    {
        private byte[] _data;

        internal SwapForceQuests(byte[] data)
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

        internal uint BadguyBasher
        {
            get { return Utils.ReadBits(_data, 0x00, 0xA); }
            set { Utils.WriteBits(_data, 0x00, 0xA, value); }
        }

        internal uint FruitFrontiersman
        {
            get { return Utils.ReadBits(_data, 0x0A, 0x4); }
            set { Utils.WriteBits(_data, 0x0A, 0x4, value); }
        }

        internal uint FlawlessChallenger
        {
            get { return Utils.ReadBits(_data, 0x0E, 0x1); }
            set { Utils.WriteBits(_data, 0x0E, 0x1, value); }
        }

        internal uint TrueGladiator
        {
            get { return Utils.ReadBits(_data, 0x0F, 0x4); }
            set { Utils.WriteBits(_data, 0x0F, 0x4, value); }
        }

        internal uint TotallyMaxedOut
        {
            get { return Utils.ReadBits(_data, 0x13, 0x1); }
            set { Utils.WriteBits(_data, 0x13, 0x1, value); }
        }

        internal uint Elementalist
        {
            get { return Utils.ReadBits(_data, 0x14, 0xD); }
            set { Utils.WriteBits(_data, 0x14, 0xD, value); }
        }

        internal uint ElementalQuest1
        {
            get { return Utils.ReadBits(_data, 0x21, 0x8); }
            set { Utils.WriteBits(_data, 0x21, 0x8, value); }
        }

        internal uint ElementalQuest2
        {
            get { return Utils.ReadBits(_data, 0x29, 0x9); }
            set { Utils.WriteBits(_data, 0x29, 0x9, value); }
        }

        internal uint IndividualQuest
        {
            get { return Utils.ReadBits(_data, 0x32, 0x10); }
            set { Utils.WriteBits(_data, 0x32, 0x10, value); }
        }
    }
}