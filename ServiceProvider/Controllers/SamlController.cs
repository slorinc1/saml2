using Microsoft.AspNetCore.Mvc;
using SSOLibrary;
using System;
using System.Text;

namespace ServiceProvider.Controllers
{
    [Route("saml")]
    public class SamlController : Controller
    {
        private readonly IAuthnRequestXMLSerializer authnRequestXMLSerializer;

        private readonly ISamlService samlService;

        public SamlController(IAuthnRequestXMLSerializer authnRequestXMLSerializer, ISamlService samlService)
        {
            this.authnRequestXMLSerializer = authnRequestXMLSerializer;
            this.samlService = samlService;
        }

        [Route("request")]
        public IActionResult GetRequest()
        {
            var request = this.samlService.GetSamlRequest();

            var result = this.authnRequestXMLSerializer.Serialize(request);

            return Ok(result);
        }

        [Route("request64")]
        public IActionResult GetRequestBase64()
        {
            var request = this.samlService.GetSamlRequest();

            var result = this.authnRequestXMLSerializer.Serialize(request);

            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(result));

            return Ok(base64);
        }
    }
}
