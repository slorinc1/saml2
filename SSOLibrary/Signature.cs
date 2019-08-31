using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SSOLibrary
{
    [XmlRoot(Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public class Signature
    {
        [XmlElement]
        public SignedInfo SignedInfo { get; set; }

        [XmlElement]
        public string SignatureValue { get; set; }

        [XmlElement]
        public KeyInfo KeyInfo { get; set; }
    }

    public class KeyInfo
    {
        [XmlElement]
        public X509Data X509Data { get; set; }
    }

    public class X509Data
    {
        [XmlElement]
        public X509IssuerSerial X509IssuerSerial { get; set; }
    }

    public class X509IssuerSerial
    {
        [XmlElement]
        public string X509IssuerName { get; set; }

        [XmlElement]
        public string X509SerialNumber { get; set; }
    }

    public class SignedInfo
    {
        [XmlElement]
        public CanonicalizationMethod CanonicalizationMethod { get; set; }

        [XmlElement]
        public SignatureMethod SignatureMethod { get; set; }

        [XmlElement]
        public Reference Reference { get; set; }
    }

    public class Reference
    {
        [XmlAttribute]
        public string URI { get; set; }

        [XmlArray]
        public Transforms Transforms { get; set; }

        [XmlElement]
        public DigestMethod DigestMethod { get; set; }

        [XmlElement]
        public string DigestValue { get; set; }
    }

    public class Transforms : List<Transform>
    {
    }

    public class Transform
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }

    public class DigestMethod
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }

    public class CanonicalizationMethod
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }

    public class SignatureMethod
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
}
