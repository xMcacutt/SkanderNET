using System.Collections.Generic;

namespace SkanderNET
{
    internal static class WebCode
    {
        private const string WebCodeAlphabet = "23456789BCDFGHJKLMNPQRSTVWXYZ";
        
        public static string ToWebCode(this ulong tradingCardId)
        {
            if (tradingCardId == 0)
                return WebCodeAlphabet[0].ToString();

            var chars = new List<char>();

            while (tradingCardId > 0)
            {
                var remainder = (int)(tradingCardId % 29);
                chars.Add(WebCodeAlphabet[remainder]);
                tradingCardId /= 29;
            }

            chars.Reverse();
            return new string(chars.ToArray());
        }
    }
}