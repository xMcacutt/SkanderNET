using System;

namespace SkanderNET.Exceptions
{
    internal class ChecksumGenerationFailureException : Exception
    {
        public ChecksumGenerationFailureException(string message) : base(message)
        {
            
        }
    }
}