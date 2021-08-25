using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Xades.Net.Lib.Common
{
    public class VerificationParameters
    {
        public string InputPath { get; set; }
        public X509Certificate2 VerificationCertificate { get; set; }
        public bool VerifyCertificate { get; set; }
    }
}
