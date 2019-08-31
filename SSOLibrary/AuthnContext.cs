using System.Xml.Serialization;

namespace SSOLibrary
{
    public class AuthnContext
    {
        [XmlElement]
        public string AuthnContextClassRef { get; set; }
    }
}
