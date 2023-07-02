using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Xades.Net.Lib.Common.Exceptions
{
    [Serializable]
    class InvalidDocumentException : Exception
    {
        public InvalidDocumentException()
        {
        }

        public InvalidDocumentException(string message)
            : base(message)
        {
        }

        public InvalidDocumentException(string message,
                                        Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidDocumentException(SerializationInfo info,
                                           StreamingContext context)
            : base(info, context)
        {
        }
    }
}
