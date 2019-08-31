using System.Xml.Serialization;

namespace SSOLibrary
{
    public class Attribute
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string NameFormat { get; set; }

        [XmlElement]
        public AttributeValue AttributeValue { get; set; }
    }
}
