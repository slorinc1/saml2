using System;
using System.Xml.Serialization;

namespace SSOLibrary
{
    public class AuthnStatement
    {
        [XmlAttribute]
        public DateTime AuthnInstant { get; set; }

        [XmlAttribute]
        public DateTime SessionNotOnOrAfter { get; set; }

        [XmlAttribute]
        public string SessionIndex { get; set; }

        [XmlElement]
        public AuthnContext AuthnContext { get; set; }
    }
}
