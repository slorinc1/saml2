using System;
using System.Xml.Serialization;

namespace SSOLibrary
{
    [XmlRoot("Response", Namespace = SAMLContants.SAMLP_NAMESPACE)]
    public class UnsignedSAMLResponse : ISignable
    {
        [XmlElement(Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Signature Signature { get; set; }
        
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string Version { get; set; }

        [XmlAttribute]
        public DateTime IssueInstant { get; set; }

        [XmlAttribute]
        public string Destination { get; set; }

        [XmlAttribute]
        public string InResponseTo { get; set; }

        [XmlElement]
        public string Issuer { get; set; }

        public Status Status { get; set; }

        public Assertion Assertion { get; set; }
    }
}
