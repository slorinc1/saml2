using System;
using System.Xml.Serialization;

namespace ServiceProvider.Models.SAML
{
    [XmlRoot("AuthnRequest", Namespace = SAMLContants.SAMLP_NAMESPACE)]
    public class AuthnRequest
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string Version { get; set; }

        [XmlAttribute]
        public string ProviderName { get; set; }

        [XmlAttribute]
        public DateTime IssueInstant { get; set; }

        [XmlAttribute]
        public string Destination { get; set; }

        [XmlAttribute]
        public string ProtocolBinding { get; set; }

        [XmlAttribute]
        public string AssertionConsumerServiceURL { get; set; }

        [XmlElement(Namespace = SAMLContants.SAML_NAMESPACE)]
        public string Issuer { get; set; }

        public NameIDPolicy NameIDPolicy { get; set; }

        public RequestedAuthnContext RequestedAuthnContext { get; set; }
    }

    [XmlRoot("NameIDPolicy", Namespace = SAMLContants.SAMLP_NAMESPACE)]
    public class NameIDPolicy
    {
        [XmlAttribute]
        public string Format { get; set; }

        [XmlAttribute]
        public bool AllowCreate { get; set; }
    }

    [XmlRoot("RequestedAuthnContext", Namespace = SAMLContants.SAMLP_NAMESPACE)]
    public class RequestedAuthnContext
    {
        [XmlAttribute]
        public string Comparison { get; set; }

        [XmlElement(Namespace = SAMLContants.SAML_NAMESPACE)]
        public string AuthnContextClassRef { get; set; }
    }
}
