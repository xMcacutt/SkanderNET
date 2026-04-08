using System;

namespace SkanderNET.Exceptions
{
    internal class UnknownToyException : Exception
    {
        public UnknownToyException(string message) : base(message)
        {
            
        }
    }
}