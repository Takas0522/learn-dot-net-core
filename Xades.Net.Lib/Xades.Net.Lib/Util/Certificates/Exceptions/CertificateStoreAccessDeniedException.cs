using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Xades.Net.Lib.Util.Certificates.Exceptions
{
    [Serializable]
    public class CertificateStoreAccessDeniedException : Exception
    {
        public CertificateStoreAccessDeniedException()
        {
        }

        public CertificateStoreAccessDeniedException(string message)
            : base(message)
        {
        }

        public CertificateStoreAccessDeniedException(string message,
                                                     Exception innerException)
            : base(message, innerException)
        {
        }

        protected CertificateStoreAccessDeniedException(SerializationInfo info,
                                                        StreamingContext context)
            : base(info, context)
        {
        }

    }
}
