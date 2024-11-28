using System;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using ACMESharp.Crypto;
using ACMESharp.Crypto.JOSE;
using ACMESharp.Crypto.JOSE.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACMESharp.UnitTests
{
    [TestClass]
    public class JwsHelperTests
    {
        [TestMethod]
        [TestCategory("skipCI")]
        public void ComputeThumbPrint()
        {
            string expectedThumbprint = "IsUn6_e04MaShXFIISMp4kG62LWzMIPy_MvSA5pJgX8";
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(JwsTests.GetRsaParamsForRfc7515Example_A_2_1());
            RSJwsTool signer = new RSJwsTool();
            signer.Init();
            signer.Import(rsa.ToXmlString(true));

            var thumbprint1 = CryptoHelper.Base64.UrlEncode(JwsHelper.ComputeThumbprint(signer, SHA256.Create()));
            Console.WriteLine(thumbprint1.ToString());
            Assert.AreEqual(expectedThumbprint, thumbprint1.ToString());
        }

        [TestMethod]
        [TestCategory("skipCI")]
        public void ComputeKeyAuthorization()
        {
            string token = "YjPipZelTF16MQ8VZNe2axbpEDoNIiUvi9E376p9arQ";
            string expected = "YjPipZelTF16MQ8VZNe2axbpEDoNIiUvi9E376p9arQ.IsUn6_e04MaShXFIISMp4kG62LWzMIPy_MvSA5pJgX8";
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(JwsTests.GetRsaParamsForRfc7515Example_A_2_1());
            RSJwsTool signer = new RSJwsTool();
            signer.Init();
            signer.Import(rsa.ToXmlString(true));

            var output1 = JwsHelper.ComputeKeyAuthorization(signer, token);
            Console.WriteLine(output1.ToString());
            Assert.AreEqual(expected, output1.ToString());
        }

        [TestMethod]
        [TestCategory("skipCI")]
        public void ComputeKeyAuthorizationDigest()
        {
            string token = "3YcHMW5bZZmnnxSHeRo5Qvs3Wr0LHgT0QVU6D2cmcDQ";
            string expected = "ZYZw3b-W1CXzdH2IENuRFCtoGRWyHmD6nJKi8YIxcbM";
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(JwsTests.GetRsaParamsForRfc7515Example_A_2_1());
            RSJwsTool signer = new RSJwsTool();
            signer.Init();
            signer.Import(rsa.ToXmlString(true));

            var output1 = JwsHelper.ComputeKeyAuthorizationDigest(signer, token);
            Console.WriteLine(output1.ToString());
            Assert.AreEqual(expected, output1.ToString());
        }


        [TestMethod]
        [TestCategory("skipCI")]
        public void TestSignFlagJson()
        {
            JwsHelper.SigningDelegate sigFunc = (x) =>
            {
                using var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(JwsTests.GetRsaParamsForRfc7515Example_A_2_1());
                using var sha256 = SHA256.Create();
                return rsa.SignData(x.ToArray(), sha256);
            };

            object protectedSample = new // From the RFC example
            {
                alg = "RS256"
            };
            object headerSample = new // From the RFC example
            {
                kid = "2010-12-29"
            };
            string payloadSample = // From the RFC example
                    "{\"iss\":\"joe\",\r\n" +
                    " \"exp\":1300819380,\r\n" +
                    " \"http://example.com/is_root\":true}";

            var wsRegex = new Regex("\\s+");
            var sigExpected = // Derived from the RFC example in A.6.4
                    wsRegex.Replace(@"{
                        ""header"":{""kid"":""2010-12-29""},
                        ""protected"":""eyJhbGciOiJSUzI1NiJ9"",
                        ""payload"":""eyJpc3MiOiJqb2UiLA0KICJleHAiOjEzMDA4MTkzODAsDQogImh0dHA6Ly9leGFtcGxlLmNvbS9pc19yb290Ijp0cnVlfQ"",
                        ""signature"":
                            ""cC4hiUPoj9Eetdgtv3hF80EGrhuB__dzERat0XF9g2VtQgr9PJbu3XOiZj5RZ
                            mh7AAuHIm4Bh-0Qc_lF5YKt_O8W2Fp5jujGbds9uJdbF9CUAr7t1dnZcAcQjb
                            KBYNX4BAynRFdiuB--f_nZLgrnbyTyWzO75vRK5h6xBArLIARNPvkSjtQBMHl
                            b1L07Qe7K0GarZRmB_eSN9383LcOLn6_dO--xi12jzDwusC-eOkHWEsqtFZES
                            c6BfI7noOPqvhJ1phCnvWh6IeYI2w9QOYEUipUTI8np6LbgGY9Fs98rqVt5AX
                            LIhWkWywlVmtVrBp0igcN_IoypGlUPQGe77Rw""
                    }", "");
            var sigActual = wsRegex.Replace(JwsHelper.SignFlatJson(
                    sigFunc, payloadSample, protectedSample, headerSample), "");
            Assert.AreEqual(sigExpected, sigActual);
        }
    }
}
