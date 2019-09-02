using Microsoft.AspNetCore.Mvc;
using ServiceProvider.Services;
using SSOLibrary;
using SSOLibrary.Services;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace ServiceProvider.Controllers
{
    [Route("saml")]
    public class SamlController : Controller
    {
        private readonly ISamlXMLSerializer authnRequestXMLSerializer;

        private readonly ISamlService samlService;

        private readonly ICertificateService certificateService;

        private readonly IXmlSignatureValidationService xmlSignatureValidationService;

        public SamlController(ISamlXMLSerializer authnRequestXMLSerializer, ISamlService samlService)
        {
            this.authnRequestXMLSerializer = authnRequestXMLSerializer;
            this.samlService = samlService;
            this.certificateService = new CertificateService();
            xmlSignatureValidationService = new XmlSignatureValidationService();
        }

        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        [Route("request")]
        public IActionResult GetRequest()
        {
            var request = this.samlService.GetSamlRequest();
            var result = this.authnRequestXMLSerializer.Serialize(request);

            var cert = this.certificateService.GetCertificate("CN=IdentityCert");
            var signatureService = new SignatureService(this.authnRequestXMLSerializer, request);
            var signedRequest = signatureService.GetSignedXml(result, cert);

            var compressed = CompressString(signedRequest);
            return Redirect($"https://localhost:44320/saml/response?base64=true&samlRequest={compressed}");
        }

        [Route("assertion")]
        [HttpPost]
        public IActionResult VerifyAssertion([FromBody]string assertion)
        {
            var signedAssertionBytes = Convert.FromBase64String(assertion);
            var signedAssertion = Encoding.UTF8.GetString(signedAssertionBytes);
            FileStream fs = new FileStream(@"C:\assert.xml", FileMode.Create);
            var bytes = Encoding.UTF8.GetBytes(signedAssertion);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            var response = this.authnRequestXMLSerializer.Deserialize<UnsignedSAMLResponse>(signedAssertion);

            var cert = this.certificateService.GetCertificate("CN=IdentityCert");

            var isValid = this.xmlSignatureValidationService.Validate(signedAssertion, cert);

            if (isValid)
            {
                var email = response.Assertion.AttributeStatement.Where(x => x.Name == "email").FirstOrDefault().AttributeValue;

                Ok(email);
            }

            return Ok("Could not validate");
        }
    }
}
