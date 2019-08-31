using System.Xml.Serialization;

namespace SSOLibrary
{
    public class Status
    {
        [XmlElement]
        public StatusCode StatusCode { get; set; }
    }
}
