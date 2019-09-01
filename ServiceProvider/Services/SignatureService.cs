using SSOLibrary;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Serialization;

namespace ServiceProvider.Services
{
    public class SignatureService : ISignatureService
    {
        readonly ISamlXMLSerializer xmlSerializer;

        readonly ISignable signableObject;

        public SignatureService(ISamlXMLSerializer xmlSerializer, ISignable unsignedObject)
        {
            this.xmlSerializer = xmlSerializer;
            this.signableObject = unsignedObject;
        }

        public string GetSignedXml(string xmlDocument, X509Certificate2 cert)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlDocument);

            SignedXml signedXml = new SignedXml(doc)
            {
                SigningKey = cert.PrivateKey
            };

            System.Security.Cryptography.Xml.Reference reference = new System.Security.Cryptography.Xml.Reference
            {
                Uri = ""
            };
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

            var response = xmlSerializer.Deserialize<SSOLibrary.Signature>(xmlDigitalSignature.OuterXml);

            signableObject.Signature = response;
            var signedRequest = this.xmlSerializer.Serialize(signableObject.GetType(), signableObject);

            return signedRequest;
        }
    }
}
