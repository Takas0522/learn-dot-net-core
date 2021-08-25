using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Xades.Net.Lib.Common.Exceptions
{
    [Serializable]
    class InvalidParameterException : Exception
    {
        public InvalidParameterException()
        {
        }

        public InvalidParameterException(string message) : base(message)
        {
        }

        public InvalidParameterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidParameterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
