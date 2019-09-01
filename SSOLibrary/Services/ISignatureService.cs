using System.Security.Cryptography.X509Certificates;

namespace SSOLibrary.Services
{
    public interface ISignatureService
    {
        string GetSignedXml(string xmlDocument, X509Certificate2 cert);
    }
}
