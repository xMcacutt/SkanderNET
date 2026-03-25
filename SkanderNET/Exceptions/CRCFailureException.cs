using System;

namespace SkanderNET
{
    internal class CRCFailureException : Exception
    {
        public CRCFailureException(string message) : base(message)
        {
            
        }
    }
}