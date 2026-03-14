using System;

namespace SkanderNET
{
    internal class SkylanderNotLoadedException : Exception
    {
        public SkylanderNotLoadedException(string message) : base(message)
        {
            
        }
    }
}