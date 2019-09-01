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

namespace IdentityProvider.Controllers
{
    [Route("saml")]
    public class SamlController : Controller
    {
        readonly ISamlXMLSerializer authnRequestXMLSerializer;
        readonly ICertificateService certificateService;
        readonly IXmlSignatureValidationService xmlSignatureValidation;
        readonly IAssertionService assertionService;

        public SamlController(ISamlXMLSerializer authnRequestXMLSerializer)
        {
            this.authnRequestXMLSerializer = authnRequestXMLSerializer;
            this.certificateService = new CertificateService();
            this.xmlSignatureValidation = new XmlSignatureValidationService();
            this.assertionService = new AssertionService(authnRequestXMLSerializer);
        }

        [Route("response")]
        [HttpPost]
        public IActionResult GetResponse([FromBody]string samlrequest)
        {
            var signedSamlBytes = Convert.FromBase64String(samlrequest);
            var signedSaml = Encoding.UTF8.GetString(signedSamlBytes);

            var cert = this.certificateService.GetCertificate("CN=IdentityCert");
            var isValid = xmlSignatureValidation.Validate(signedSaml, cert);

            if (isValid)
            {
                return Ok(this.assertionService.GetAssertion(signedSaml));
            }

            return Ok("Invalid signature.");
        }
    }
}
