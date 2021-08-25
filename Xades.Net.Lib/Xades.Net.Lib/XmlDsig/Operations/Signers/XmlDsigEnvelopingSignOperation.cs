using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Xades.Net.Lib.Common.Exceptions;

namespace Xades.Net.Lib.XmlDsig.Operations.Signers
{
    internal class XmlDsigEnvelopingSignOperation : XmlDsigSignOperation
    {
        protected override void CreateAndAddReferenceTo(SignedXml signedXml, XmlDocument document, string inputPath, string xpathToNodeToSign)
        {
            if (signedXml == null)
            {
                throw new InvalidParameterException("Signed Xml cannot be null");
            }
            if (document == null)
            {
                throw new InvalidParameterException("Xml document cannot be null");
            }
            if (document.DocumentElement == null)
            {
                throw new InvalidParameterException("Xml document must have root element");
            }

            var signatureReference = new Reference(File.Open(@"C:\temp\hoge.txt", FileMode.Open));
            signatureReference.Uri = "./hoge.txt";
            signatureReference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";

            ////var signatureReference = new Reference("#documentdata");
            //signatureReference.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(signatureReference);
            //var dataObject = new DataObject("documentdata", "", "", document.DocumentElement);
            //signedXml.AddObject(dataObject); // いらない
        }
        protected override void AddCanonicalizationMethodTo(SignedXml signedXml)
        {
            //signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
        }
    }
}
