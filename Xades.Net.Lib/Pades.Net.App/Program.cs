using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.Collections;

namespace Pades.Net.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new CertificateClient(new Uri(@"<KeyVaultUrl>"), new DefaultAzureCredential());
            var res = client.DownloadCertificateAsync("<CertName>").Result;
            var cert = res.Value;

            var obj = new AdobePdfSigner();
            obj.Sign(@"<Input PDF File Path>", @"<Output PDf FilePath>", cert);
        }
    }

    public class AdobePdfSigner
    {

        public void Sign(string inputFilePath, string outputPath, X509Certificate2 certificate)
        {
            PdfReader reader = new PdfReader(inputFilePath);
            var stamper = PdfStamper.CreateSignature(reader, new FileStream(outputPath, FileMode.Create), '\0', null, true);
            var cp = new Org.BouncyCastle.X509.X509CertificateParser();
            var chain = new[] { cp.ReadCertificate(certificate.RawData) };
            var sig = stamper.SignatureAppearance;
            SetSigPosition(sig, reader.AcroFields.GetSignatureNames().Count);
            SetSigText(sig, chain);
            SetSigCryptoFromX509(sig, certificate, chain);
        }

        private static void SetSigPosition(PdfSignatureAppearance sigAppearance, int oldSigCount)
        {
            float llx = (100 + 20) * (oldSigCount % 5),
                    lly = (25 + 20) * (oldSigCount / 5),
                    urx = llx + 100,
                    ury = lly + 25;
            sigAppearance.SetVisibleSignature(new Rectangle(llx, lly, urx, ury), 1, null);
        }

        private static void SetSigText(PdfSignatureAppearance sigAppearance, IList<Org.BouncyCastle.X509.X509Certificate> chain)
        {
            sigAppearance.Layer2Text = "…";
        }

        private static void SetSigCryptoFromX509(PdfSignatureAppearance sigAppearance, X509Certificate2 card, Org.BouncyCastle.X509.X509Certificate[] chain)
        {
            sigAppearance.SetCrypto(null, chain, null, PdfSignatureAppearance.WincerSigned);
            var dic = new PdfSignature(PdfName.AdobePpkms, PdfName.AdbePkcs7Sha1)
            {
                Date = new PdfDate(sigAppearance.SignDate),
                Name = "CE NAME",
                Reason = sigAppearance.Reason,
                Location = sigAppearance.Location
            };
            sigAppearance.CryptoDictionary = dic;
            const int csize = 4000;
            var exc = new Dictionary<PdfName, int> { { PdfName.Contents, csize * 2 + 2 } };
            var excHash = exc.ToHashtable();
            sigAppearance.PreClose(excHash);

            HashAlgorithm sha = new SHA1CryptoServiceProvider();

            var s = sigAppearance.RangeStream;
            int read;
            var buff = new byte[8192];
            while ((read = s.Read(buff, 0, 8192)) > 0)
            {
                sha.TransformBlock(buff, 0, read, buff, 0);
            }
            sha.TransformFinalBlock(buff, 0, 0);
            var pk = SignMsg(sha.Hash, card, false);

            var outc = new byte[csize];

            var dic2 = new PdfDictionary();

            Array.Copy(pk, 0, outc, 0, pk.Length);

            dic2.Put(PdfName.Contents, new PdfString(outc).SetHexWriting(true));

            sigAppearance.Close(dic2);
        }

        private static byte[] SignMsg(Byte[] msg, X509Certificate2 signerCert, bool detached)
        {

            var contentInfo = new ContentInfo(msg);

            //  Instantiate SignedCms object with the ContentInfo above.
            //  Has default SubjectIdentifierType IssuerAndSerialNumber.
            var signedCms = new SignedCms(contentInfo, detached);

            //  Formulate a CmsSigner object for the signer.
            var cmsSigner = new CmsSigner(signerCert);

            // Include the following line if the top certificate in the
            // smartcard is not in the trusted list.
            cmsSigner.IncludeOption = X509IncludeOption.EndCertOnly;

            //  Sign the CMS/PKCS #7 message. The second argument is
            //  needed to ask for the pin.
            signedCms.ComputeSignature(cmsSigner, false);

            //  Encode the CMS/PKCS #7 message.
            return signedCms.Encode();
        }

    }

    /// <summary>
    /// Dictionary 型の拡張メソッドを管理するクラス
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 指定された Dictionary<TKey, TValue> を Hashtable に変換します
        /// </summary>
        /// <param name="self">Dictionary<TKey, TValue> 型のインスタンス</param>
        /// <returns>Hashtable 型のインスタンス</returns>
        public static Hashtable ToHashtable<TKey, TValue>(this Dictionary<TKey, TValue> self)
        {
            var result = new Hashtable();
            foreach (var n in self)
            {
                result[n.Key] = n.Value;
            }
            return result;
        }
    }
}
