using System.Xml.Serialization;

namespace SSOLibrary
{
    public class Subject
    {
        [XmlElement]
        public NameID NameID { get; set; }

        [XmlElement]
        public SubjectConfirmation SubjectConfirmation { get; set; }
    }

}
