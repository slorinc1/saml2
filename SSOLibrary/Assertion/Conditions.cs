using System;
using System.Xml.Serialization;

namespace SSOLibrary
{
    public class Conditions
    {
        [XmlAttribute]
        public DateTime NotBefore { get; set; }

        [XmlAttribute]
        public DateTime NotOnOrAfter { get; set; }

        public AudienceRestriction AudienceRestriction { get; set; }
    }
}
