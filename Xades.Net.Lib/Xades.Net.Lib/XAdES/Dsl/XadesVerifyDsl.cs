using System;
using System.Collections.Generic;
using System.Text;
using Xades.Net.Lib.Common;
using Xades.Net.Lib.XAdES.Operations;
using Xades.Net.Lib.XmlDsig.Common;

namespace Xades.Net.Lib.XAdES.Dsl
{
    public class XadesVerifyDsl
    {
        private readonly VerificationParameters _parameters = new VerificationParameters();

        public XadesVerifyDsl SignaturePath(string signaturePath)
        {
            _parameters.InputPath = signaturePath;
            return this;
        }

        public XadesVerifyDsl AlsoVerifyCertificate()
        {
            _parameters.VerifyCertificate = true;
            return this;
        }
        public XadesVerifyDsl DoNotVerifyCertificate()
        {
            _parameters.VerifyCertificate = false;
            return this;
        }

        public void Perform()
        {
            XadesVerifyOperation.Verify(_parameters);
        }

        public VerificationResults PerformAndGetResults()
        {
            return XadesVerifyOperation.VerifyAndGetResults(_parameters);
        }
    }
}
