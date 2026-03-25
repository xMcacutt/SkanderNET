using System;

namespace SkanderNET
{
    internal class UsbCommunicationsException : Exception
    {
        public UsbCommunicationsException(string message) : base(message)
        {
            
        }
    }
}