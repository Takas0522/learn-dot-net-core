using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Xades.Net.Lib.Util.Xml;

namespace Xades.Net.Lib.Schemas
{
    public class SignatureSchemas
    {
        private static readonly Dictionary<string, string> Schemas = new Dictionary<string, string>();

        private SignatureSchemas()
        {
        }

        public static XmlReader Get(string schemaUri)
        {
            LoadSchemasIfNeeded();
            if (Schemas.ContainsKey(schemaUri))
            {
                var settings = new XmlReaderSettings { ProhibitDtd = false };
                var xmlReader = XmlReader.Create(new StringReader(Schemas[schemaUri]), settings);
                return xmlReader;
            }
            return null;
        }

        private static void LoadSchemasIfNeeded()
        {
            if (Schemas.Count != 0) return;
            Schemas[SignedXml.XmlDsigNamespaceUrl] = Properties.Resources.xmldsig_core_schema;
            Schemas[XmlHelper.XmlSchemaUrl] = Properties.Resources.XMLSchema;
        }
    }
}
