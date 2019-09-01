using System.Security.Cryptography.X509Certificates;

namespace IdentityProvider.Services
{
    public interface IXmlSignatureValidationService
    {
        bool Validate(string xml, X509Certificate2 cert);
    }
}
