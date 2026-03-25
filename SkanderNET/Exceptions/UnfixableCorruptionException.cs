using System;

namespace SkanderNET
{
    internal class UnfixableCorruptionException : Exception
    {
        public UnfixableCorruptionException(string message) : base(message)
        {
            
        }
    }
}