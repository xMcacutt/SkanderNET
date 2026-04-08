using System;

namespace SkanderNET.Exceptions
{
    internal class FormatRequiredException : Exception
    {
        public FormatRequiredException(string message) : base(message)
        {
            
        }
    }
}