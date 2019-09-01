using Microsoft.AspNetCore.Mvc;
using ServiceProvider.Services;
using SSOLibrary;
using SSOLibrary.Services;
using System;
using System.IO;
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

        [Route("request")]
        public IActionResult GetRequest(bool base64)
        {
            var request = this.samlService.GetSamlRequest();
            var result = this.authnRequestXMLSerializer.Serialize(request);

            var cert = this.certificateService.GetCertificate("CN=IdentityCert");
            var signatureService = new SignatureService(this.authnRequestXMLSerializer, request);
            var signedRequest = signatureService.GetSignedXml(result, cert);

            if (base64)
            {
                var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(signedRequest));

                return Ok(base64String);
            }

            return Ok(signedRequest);
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
