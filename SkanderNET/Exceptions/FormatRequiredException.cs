using System;

namespace SkanderNET
{
    public class FormatRequiredException : Exception
    {
        public FormatRequiredException(string message) : base(message)
        {
            
        }
    }
}