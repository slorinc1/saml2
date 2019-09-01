using System.Xml.Serialization;

namespace SSOLibrary
{
    public class SubjectConfirmation
    {
        [XmlAttribute]
        public string Method { get; set; }

        public SubjectConfirmationData SubjectConfirmationData { get; set; }
    }

}
