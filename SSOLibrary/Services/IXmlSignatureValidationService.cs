using System.Security.Cryptography.X509Certificates;

namespace SSOLibrary.Services
{
    public interface IXmlSignatureValidationService
    {
        bool Validate(string xml, X509Certificate2 cert);
    }
}
