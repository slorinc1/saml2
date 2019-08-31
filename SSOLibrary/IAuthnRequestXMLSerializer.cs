using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SSOLibrary
{
    public interface IAuthnRequestXMLSerializer
    {
        string Serialize(AuthnRequest authnRequest);
    }

    public class AuthnRequestXMLSerializer : IAuthnRequestXMLSerializer
    {
        public string Serialize(AuthnRequest authnRequest)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("samlp", SAMLContants.SAMLP_NAMESPACE);
            ns.Add("saml", SAMLContants.SAML_NAMESPACE);

            var serializer = new XmlSerializer(authnRequest.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, authnRequest, ns);

                return stringWriter.ToString();
            }
        }
    }
}
