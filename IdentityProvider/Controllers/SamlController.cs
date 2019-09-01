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

        [Route("response")]
        [HttpPost]
        public IActionResult GetResponse([FromBody]string samlrequest, bool base64)
        {
            var signedSamlBytes = Convert.FromBase64String(samlrequest);
            var signedSaml = Encoding.UTF8.GetString(signedSamlBytes);

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
