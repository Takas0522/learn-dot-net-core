using System;
using System.Collections.Generic;
using System.Text;

namespace Xades.Net.Lib.Common.Exceptions
{
    public class InvalidSignedDocumentException : Exception
    {
        public InvalidSignedDocumentException(string message) : base(message)
        {
        }
    }
}
