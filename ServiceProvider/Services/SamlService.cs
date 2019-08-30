using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ServiceProvider.Services
{
    public class AuthnRequestProperties
    {
        public const string AuthnRequest = "AuthnRequest";
        public const string Issuer = "Issuer";
        public const string NameIDPolicy = "NameIDPolicy";
        public const string RequestedAuthnContext = "RequestedAuthnContext";
        public const string AuthnContextClassRef = "AuthnContextClassRef";
    }

    public class AuthnRequestAttributes
    {
        public const string ID = "ID";
        public const string Version = "Version";
        public const string ProviderName = "ProviderName";
        public const string IssueInstant = "IssueInstant";
        public const string Destination = "Destination";
        public const string ProtocolBinding = "ProtocolBinding";
        public const string AssertionConsumerServiceURL = "AssertionConsumerServiceURL";
        public const string Format = "Format";
        public const string AllowCreate = "AllowCreate";
        public const string Comparison = "Comparison";
    }

    public class SAMLContants
    {
        public const string SAMLP_NAMESPACE = "urn:oasis:names:tc:SAML:2.0:protocol";
        public const string SAML_NAMESPACE = "urn:oasis:names:tc:SAML:2.0:assertion";
        public const string Version = "2.0";
        public const string ProtocolBinding = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST";
        public const string Format = "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress";
        public const string AuthnContextClassRef = "urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport";
    }

    public class SamlService
    {
        public SamlService()
        {

        }

        public string GetSamlRequest()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement elem = doc.CreateElement("samlp", AuthnRequestProperties.AuthnRequest, SAMLContants.SAMLP_NAMESPACE);
            elem.Attributes.Append(doc.);

            var authnContextClassRef = new XElement($"saml:{AuthnRequestProperties.AuthnContextClassRef}, SAMLContants.AuthnContextClassRef);

            return xmlDocument.ToString();
        }
    }
}
