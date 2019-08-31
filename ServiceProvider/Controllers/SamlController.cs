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
        public IActionResult GetRequest(bool base64)
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

            if (base64)
            {
                var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(signedRequest));

                return Ok(base64String);
            }

            return Ok(signedRequest);
        }
    }
}
