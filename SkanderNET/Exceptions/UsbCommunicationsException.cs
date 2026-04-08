using System;

namespace SkanderNET.Exceptions
{
    internal class UsbCommunicationsException : Exception
    {
        public UsbCommunicationsException(string message) : base(message)
        {
            
        }
    }
}