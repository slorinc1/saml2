using System.Xml.Serialization;

namespace SSOLibrary
{
    public class StatusCode
    {
        [XmlAttribute]
        public string Value { get; set; }
    }
}
