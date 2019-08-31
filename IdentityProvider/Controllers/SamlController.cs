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

namespace IdentityProvider.Controllers
{
    [Route("saml")]
    public class SamlController : Controller
    {
        IAuthnRequestXMLSerializer authnRequestXMLSerializer;

        public SamlController(IAuthnRequestXMLSerializer authnRequestXMLSerializer)
        {
            this.authnRequestXMLSerializer = authnRequestXMLSerializer;
        }

        [Route("response")]
        [HttpPost]
        public IActionResult GetResponse([FromBody]string samlrequest)
        {
            var signedSamlBytes = Convert.FromBase64String(samlrequest);
            var signedSaml = Encoding.UTF8.GetString(signedSamlBytes);

            var doc = new XmlDocument();
            doc.LoadXml(signedSaml);

            X509Certificate2 cert = new X509Certificate2(@"C:\Users\SandorLorinc\Desktop\saml2\ServiceProvider\ident.pfx", "alma");

            if (ValidateSoapBodySignature(doc, cert))
            {
                return Ok(CreateResponse("requestid"));
            }
            else
            {
                return Ok("Could not validate");
            }
        }

        public bool ValidateSoapBodySignature(XmlDocument doc, X509Certificate2 cert)
        {
            doc.DocumentElement.SetAttribute("id", "AuthnRequest");
            
            SignedXml sdoc = new SignedXml(doc);
            
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("samlp", SAMLContants.SAMLP_NAMESPACE);
            XmlElement body = doc.DocumentElement.SelectSingleNode(@"//samlp:Signature", ns) as XmlElement;
            sdoc.LoadXml(body);
            
            bool result = sdoc.CheckSignature(cert, true);

            return result;
        }

        public string CreateResponse(string id)
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
                ID = Guid.NewGuid().ToString(),
                InResponseTo = id,
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
                                InResponseTo = id,
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

            return result;
        }
    }
}
