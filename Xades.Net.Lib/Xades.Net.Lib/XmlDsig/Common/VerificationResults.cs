using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Xades.Net.Lib.XmlDsig.Common
{
    public class VerificationResults
    {
        public string Timestamp { get; set; }
        public XmlDocument OriginalDocument { get; set; }
        public X509Certificate2 SigningCertificate { get; set; }
    }
}
