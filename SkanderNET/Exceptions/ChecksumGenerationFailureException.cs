using System;

namespace SkanderNET
{
    internal class ChecksumGenerationFailureException : Exception
    {
        public ChecksumGenerationFailureException(string message) : base(message)
        {
            
        }
    }
}