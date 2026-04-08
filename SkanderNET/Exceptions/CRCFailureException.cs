using System;

namespace SkanderNET.Exceptions
{
    internal class CRCFailureException : Exception
    {
        public CRCFailureException(string message) : base(message)
        {
            
        }
    }
}