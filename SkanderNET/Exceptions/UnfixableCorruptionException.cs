using System;

namespace SkanderNET.Exceptions
{
    internal class UnfixableCorruptionException : Exception
    {
        public UnfixableCorruptionException(string message) : base(message)
        {
            
        }
    }
}