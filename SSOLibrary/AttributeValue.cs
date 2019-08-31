using System.Xml.Serialization;

namespace SSOLibrary
{
    public class AttributeValue
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "type", Namespace = "typenamscpace")]
        public string Type { get; set; }
    }
}
