namespace SkanderNET
{
    public class CRC16_IBM3740
    {
        private const ushort poly = 0x1021;
        private const ushort init = 0xFFFF;
        
        public static ushort Generate(byte[] data)
        {
            var crc = init;
            foreach (var b in data)
            {
                crc ^= (ushort)(b << 8);
                for (var _ = 0; _ < 8; _++)
                {
                    if ((crc & 0x8000) != 0)
                        crc = (ushort)((crc << 1) ^ poly);
                    else
                        crc = (ushort)(crc << 1);
                }
            }
            return crc;
        }
    }
}