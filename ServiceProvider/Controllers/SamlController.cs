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

        [Route("response")]
        public IActionResult GetResponse()
        {
            var attributeStatement = new AttributeStatement()
            {
                new SSOLibrary.Attribute()
                {
                    AttributeValue = new AttributeValue()
                    {
                        Value = "slorinc@email.com",
                        Type = "asdasd"
                    },
                    Name = "email",
                    NameFormat = "string"
                },

                new SSOLibrary.Attribute()
                {
                    AttributeValue = new AttributeValue()
                    {
                        Value = "Lőrinc Sándor",
                        Type = "name"
                    },
                    Name = "name",
                    NameFormat = "string"
                }
            };

            var response = new UnsignedSAMLResponse()
            {
                Destination = "dest",
                ID = "id",
                InResponseTo = "asdasd",
                IssueInstant = DateTime.Now,
                Version = SAMLContants.Version,
                Status = new Status()
                {
                    StatusCode = new StatusCode()
                    {
                        Value = "alma"
                    }
                },
                Issuer = "asdasd",
                Assertion = new Assertion()
                {
                    AuthnStatement = new AuthnStatement()
                    {
                        AuthnContext = new AuthnContext()
                        {
                            AuthnContextClassRef = "asda"
                        },
                        AuthnInstant = DateTime.Now,
                        SessionIndex = "asdasd",
                        SessionNotOnOrAfter = DateTime.Now.AddDays(12)
                    },
                    ID = "adsasd",
                    IssueInstant = DateTime.Now,
                    Issuer = "asdasd",
                    Subject = new Subject()
                    {
                        NameID = new NameID()
                        {
                            Format = "format",
                            SPNameQualifier = "spname",
                            Value = "value"
                        },
                        SubjectConfirmation = new SubjectConfirmation()
                        {
                            Method = "method",
                            SubjectConfirmationData = new SubjectConfirmationData()
                            {
                                InResponseTo = "asdasd",
                                NotOnOrAfter = DateTime.Now,
                                Recipient = "recipient"
                            }
                        }
                    },
                    Version = SAMLContants.Version,
                    Conditions = new Conditions()
                    {
                        AudienceRestriction = new AudienceRestriction()
                        {
                            Audience = "audience"
                        },
                        NotBefore = DateTime.MaxValue,
                        NotOnOrAfter = DateTime.MinValue
                    },
                    AttributeStatement = attributeStatement
                }
            };

            var result = this.authnRequestXMLSerializer.Serialize(response);

            return Ok(result);
        }
    }
}
