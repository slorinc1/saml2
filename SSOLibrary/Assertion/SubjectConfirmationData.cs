using System;
using System.Xml.Serialization;

namespace SSOLibrary
{
    public class SubjectConfirmationData
    {
        [XmlAttribute]
        public DateTime NotOnOrAfter { get; set; }

        [XmlAttribute]
        public string Recipient { get; set; }

        [XmlAttribute]
        public string InResponseTo { get; set; }
    }

}
