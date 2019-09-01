using System.Security.Cryptography.X509Certificates;

namespace IdentityProvider.Services
{
    public interface ICertificateService
    {
        X509Certificate2 GetCertificate(string issuer);
    }
}
