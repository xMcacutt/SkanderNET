using System;
using System.Security.Cryptography;

namespace SkanderNET
{
    public static class SkylanderEncryption
    {
        private static readonly byte[] HashConst =
        {
            0x20,0x43,0x6F,0x70,0x79,0x72,0x69,0x67,0x68,0x74,0x20,0x28,0x43,0x29,0x20,0x32,
            0x30,0x31,0x30,0x20,0x41,0x63,0x74,0x69,0x76,0x69,0x73,0x69,0x6F,0x6E,0x2E,0x20,
            0x41,0x6C,0x6C,0x20,0x52,0x69,0x67,0x68,0x74,0x73,0x20,0x52,0x65,0x73,0x65,0x72,
            0x76,0x65,0x64,0x2E,0x20
        };

        public static byte[] DecryptBlock(byte[] sector0, uint blockIndex, byte[] encryptedBlock)
        {
            if (blockIndex < 0x08)
                return encryptedBlock;

            if ((blockIndex % 4) == 3)
                return encryptedBlock;

            byte[] key = GenerateKey(sector0, blockIndex);

            Aes aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.ECB;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.None;

            ICryptoTransform decrypter = aes.CreateDecryptor();
            byte[] result = decrypter.TransformFinalBlock(encryptedBlock, 0, 16);

            aes.Dispose();

            return result;
        }
        
        public static byte[] EncryptBlock(byte[] sector0, uint blockIndex, byte[] unencryptedBlock)
        {
            if (blockIndex < 0x08)
                return unencryptedBlock;

            if ((blockIndex % 4) == 3)
                return unencryptedBlock;

            byte[] key = GenerateKey(sector0, blockIndex);

            Aes aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.ECB;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.None;

            ICryptoTransform encrypter = aes.CreateEncryptor();
            byte[] result = encrypter.TransformFinalBlock(unencryptedBlock, 0, 16);

            aes.Dispose();

            return result;
        }

        private static byte[] GenerateKey(byte[] sector0, uint blockIndex)
        {
            byte[] buffer = new byte[0x56];
            Buffer.BlockCopy(sector0, 0, buffer, 0, 0x20);
            buffer[0x20] = (byte)blockIndex;
            Buffer.BlockCopy(HashConst, 0, buffer, 0x21, HashConst.Length);
            MD5 md5 = MD5.Create();
            byte[] key = md5.ComputeHash(buffer);
            md5.Dispose();
            return key;
        }
    }
}