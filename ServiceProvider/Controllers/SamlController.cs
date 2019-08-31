using Microsoft.AspNetCore.Mvc;
using SSOLibrary;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

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
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);

            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("samlp", SAMLContants.SAMLP_NAMESPACE);

            XmlElement body = doc.DocumentElement.SelectSingleNode(@"//samlp:AuthnRequest", ns) as XmlElement;

            // *** We'll only encode the <SOAP:Body> - add id: Reference as #Body
            body.SetAttribute("id", "AuthnRequest");

            X509Certificate2 cert = new X509Certificate2(@"C:\Users\SandorLorinc\Desktop\saml2\ServiceProvider\ident.pfx", "alma");
            SignedXml signedXml = new SignedXml(doc);
            System.Security.Cryptography.Xml.KeyInfo keyInfo = new System.Security.Cryptography.Xml.KeyInfo();

            // *** The actual key for signing - MAKE SURE THIS ISN'T NULL!
            signedXml.SigningKey = cert.PrivateKey;

            System.Security.Cryptography.Xml.Reference reference = new System.Security.Cryptography.Xml.Reference
            {
                Uri = "#AuthnRequest"  // reference id=body section in same doc
            };
            reference.AddTransform(new XmlDsigExcC14NTransform());  // required to match doc
            signedXml.AddReference(reference);

            // *** Specifically use the issuer and serial number for the data rather than the default
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data();
            keyInfoData.AddIssuerSerial(cert.Issuer, cert.GetSerialNumberString());
            keyInfo.AddClause(keyInfoData);
            signedXml.KeyInfo = keyInfo;
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.ComputeSignature();

            XmlElement signedElement = signedXml.GetXml();

            XmlElement soapSignature = doc.CreateElement("Signature");
            // *** And add our signature as content
            soapSignature.AppendChild(signedElement);

            SSOLibrary.Signature response = null;
            XmlSerializer serializer = new XmlSerializer(typeof(SSOLibrary.Signature));
            using (StringReader reader = new StringReader(soapSignature.InnerXml))
            {
                response = (SSOLibrary.Signature)serializer.Deserialize(reader);
            }
            request.Signature = response;

            var signedRequest = this.authnRequestXMLSerializer.Serialize(request);

            return Ok(signedRequest);
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
        public IActionResult GetResponse(string id)
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

            return Ok(result);
        }
    }
}
