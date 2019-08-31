using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SSOLibrary
{
    public interface IAuthnRequestXMLSerializer
    {
        string Serialize(AuthnRequest authnRequest);

        string Serialize(UnsignedSAMLResponse samlResponse);

        string Serialize(Type type, object value);
    }

    public class AuthnRequestXMLSerializer : IAuthnRequestXMLSerializer
    {
        public string Serialize(Type type, object value)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("samlp", SAMLContants.SAMLP_NAMESPACE);
            ns.Add("saml", SAMLContants.SAML_NAMESPACE);

            var serializer = new XmlSerializer(value.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, value, ns);

                return stringWriter.ToString();
            }
        }

        public string Serialize(UnsignedSAMLResponse samlResponse)
        {
            return Serialize(samlResponse.GetType(), samlResponse);
        }

        public string Serialize(AuthnRequest authnRequest)
        {
            return Serialize(authnRequest.GetType(), authnRequest);
        }
    }
}
