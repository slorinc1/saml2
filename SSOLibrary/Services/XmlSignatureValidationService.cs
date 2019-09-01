using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace SSOLibrary.Services
{
    public class XmlSignatureValidationService : IXmlSignatureValidationService
    {
        public bool Validate(string xml, X509Certificate2 cert)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            SignedXml signedXml = new SignedXml(doc);
            XmlNodeList nodeList = doc.GetElementsByTagName("Signature");
            var node = (XmlElement)nodeList[0];
            signedXml.LoadXml((XmlElement)nodeList[0]);

            return signedXml.CheckSignature(cert, true);
        }
    }
}
