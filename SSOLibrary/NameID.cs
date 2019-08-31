using System.Xml.Serialization;

namespace SSOLibrary
{
    public class NameID
    {
        [XmlAttribute]
        public string SPNameQualifier { get; set; }

        [XmlAttribute]
        public string Format { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

}
