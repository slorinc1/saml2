using Microsoft.AspNetCore.Mvc;
using ServiceProvider.Services;
using SSOLibrary;
using System;
using System.Text;

namespace ServiceProvider.Controllers
{
    [Route("saml")]
    public class SamlController : Controller
    {
        private readonly ISamlXMLSerializer authnRequestXMLSerializer;

        private readonly ISamlService samlService;

        private readonly ICertificateService certificateService;

        public SamlController(ISamlXMLSerializer authnRequestXMLSerializer, ISamlService samlService)
        {
            this.authnRequestXMLSerializer = authnRequestXMLSerializer;
            this.samlService = samlService;
            this.certificateService = new CertificateService();
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
    }
}
