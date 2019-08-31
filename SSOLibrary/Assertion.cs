using System;
using System.Xml.Serialization;

namespace SSOLibrary
{
    [XmlRoot("Assertion", Namespace = SAMLContants.SAML_NAMESPACE)]
    public class Assertion
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string Version { get; set; }

        [XmlAttribute]
        public DateTime IssueInstant { get; set; }

        [XmlElement]
        public string Issuer { get; set; }

        [XmlElement(Namespace = SAMLContants.SAML_NAMESPACE)]
        public Subject Subject { get; set; }

        public Conditions Conditions { get; set; }

        public AuthnStatement AuthnStatement { get; set; }

        [XmlArray(ElementName = "AttributeStatement")]
        public AttributeStatement AttributeStatement { get; set; }
    }
}
