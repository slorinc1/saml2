using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityProvider.Models;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using SSOLibrary;
using IdentityProvider.Services;
using SSOLibrary.Services;
using System.IO;
using System.IO.Compression;

namespace IdentityProvider.Controllers
{
    [Route("saml")]
    public class SamlController : Controller
    {
        readonly ISamlXMLSerializer authnRequestXMLSerializer;
        readonly ICertificateService certificateService;
        readonly IXmlSignatureValidationService xmlSignatureValidation;
        readonly IAssertionService assertionService;
        readonly ISignatureService signatureService;
        readonly ISamlXMLSerializer samlXMLSerializer;

        public SamlController(ISamlXMLSerializer authnRequestXMLSerializer)
        {
            this.authnRequestXMLSerializer = authnRequestXMLSerializer;
            this.certificateService = new CertificateService();
            this.xmlSignatureValidation = new XmlSignatureValidationService();
            this.assertionService = new AssertionService(authnRequestXMLSerializer);
            this.samlXMLSerializer = new SamlXMLSerializer();
        }

        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        [Route("response")]
        [HttpGet]
        public IActionResult GetResponse(string samlrequest, bool base64)
        {
            var signedSaml = DecompressString(samlrequest);

            var cert = this.certificateService.GetCertificate("CN=IdentityCert");
            var isValid = xmlSignatureValidation.Validate(signedSaml, cert);

            if (isValid)
            {
                var assertion = this.assertionService.GetAssertion(signedSaml);

                var assertionOnbject = this.samlXMLSerializer.Deserialize<UnsignedSAMLResponse>(assertion);
                assertionOnbject.Signature = null;

                var signature = new SignatureService(this.samlXMLSerializer, assertionOnbject);
                var signedAssertion = signature.GetSignedXml(assertion, cert);

                if (base64)
                {
                    var bytes = Encoding.UTF8.GetBytes(signedAssertion);
                    var base64Assertion = Convert.ToBase64String(bytes);

                    return Ok(base64Assertion);
                }
                else
                {
                    return Ok(signedAssertion);
                }
            }

            return Ok("Invalid signature.");
        }
    }
}
