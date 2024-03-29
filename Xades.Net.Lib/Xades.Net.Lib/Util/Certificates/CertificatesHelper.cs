﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xades.Net.Lib.Util.Certificates.Exceptions;

namespace Xades.Net.Lib.Util.Certificates
{
    public enum CertificateStore
    {
        My
    }

    public abstract class CertificatesHelper
    {
        public static X509Certificate2Collection GetCertificatesFrom(CertificateStore certificateStoreType)
        {
            if (CertificateStore.My.Equals(certificateStoreType))
            {
                var store = new X509Store(StoreName.My);
                store.Open(OpenFlags.ReadOnly);
                return store.Certificates;
            }
            throw new CertificateStoreAccessDeniedException(String.Format("Cannot access the certificate store {0}", certificateStoreType));
        }
    }
}
