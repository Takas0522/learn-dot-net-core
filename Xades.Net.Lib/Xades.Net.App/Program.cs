using System;
using System.Security.Cryptography.X509Certificates;
using Xades.Net.Lib.XAdES;
using Xades.Net.Lib.XAdES.Dsl;

namespace Xades.Net.App
{
    class Program
    {
        static void Main(string[] args)
        {
            XadesSignDsl sign = XadesHelper.Sign(@"<Input XML File Path>");
            var cert = new X509Certificate2(@"<PFX File Path>", "<Password>");
            sign.IncludingCertificateInSignature();
            sign.Using(cert).SettingSignatureFormat(Lib.XmlDsig.Common.XmlDsigSignatureFormat.Enveloping);
            sign.SignToFile(@"<Output XML File Path>");
        }
    }
}
