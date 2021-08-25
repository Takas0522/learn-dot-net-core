using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Xades.Net.Lib.Common;
using Xades.Net.Lib.XAdES.Common;
using Xades.Net.Lib.XAdES.Operations;
using Xades.Net.Lib.XmlDsig.Common;

namespace Xades.Net.Lib.XAdES.Dsl
{
    public class XadesSignDsl
    {
        private readonly XadesSignParameters _parameters = new XadesSignParameters();

        public XadesSignDsl InputPath(string inputPath)
        {
            _parameters.InputPath = inputPath;
            return this;
        }
        public XadesSignDsl InputXml(XmlDocument xmlDocument)
        {
            _parameters.InputXml = xmlDocument;
            return this;
        }

        public XadesSignDsl Using(X509Certificate2 certificate)
        {
            _parameters.SignatureCertificate = certificate;
            return this;
        }

        public XadesSignDsl WithProperty(string propertyName, string propertyValue)
        {
            _parameters.Properties.Add(new XmlPropertyDescriptor
            {
                Name = propertyName,
                Value = propertyValue
            });
            return this;
        }
        public XadesSignDsl WithProperty(string propertyName, string propertyValue, string propertyNameSpace)
        {
            _parameters.Properties.Add(new XmlPropertyDescriptor
            {
                Name = propertyName,
                Value = propertyValue,
                NameSpace = propertyNameSpace
            });
            return this;
        }
        public XadesSignDsl WithPropertyBuiltFromDoc(Converter<XmlDocument, XmlElement> howToCreatePropertyNodeFromDoc)
        {
            _parameters.PropertyBuilders.Add(howToCreatePropertyNodeFromDoc);
            return this;
        }
        public XadesSignDsl IncludingCertificateInSignature()
        {
            _parameters.IncludeCertificateInSignature = true;
            return this;
        }
        public XadesSignDsl DoNotIncludeCertificateInSignature()
        {
            _parameters.IncludeCertificateInSignature = false;
            return this;
        }
        public XadesSignDsl SettingSignatureFormat(XmlDsigSignatureFormat value)
        {
            _parameters.SignatureFormat = value;
            return this;
        }

        public void SignToFile(string outputPath)
        {
            _parameters.OutputPath = outputPath;
            XadesSignOperation.SignToFile(_parameters);
        }
        public XmlDocument SignAndGetXml()
        {
            return XadesSignOperation.SignAndGetXml(_parameters);
        }
    }
}
