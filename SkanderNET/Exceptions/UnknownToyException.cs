using System;

namespace SkanderNET
{
    internal class UnknownToyException : Exception
    {
        public UnknownToyException(string message) : base(message)
        {
            
        }
    }
}