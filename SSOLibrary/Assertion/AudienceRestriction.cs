using System.Xml.Serialization;

namespace SSOLibrary
{
    public class AudienceRestriction
    {
        [XmlElement]
        public string Audience { get; set; }
    }
}
