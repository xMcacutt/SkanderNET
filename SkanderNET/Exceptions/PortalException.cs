using System;

namespace SkanderNET.Exceptions
{
    internal class PortalReadException : Exception
    {
        public PortalReadException(string message) : base(message)
        {
            
        }
    }
    
    internal class PortalWriteException : Exception
    {
        public PortalWriteException(string message) : base(message)
        {
            
        }
    }
}