using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace ServiceProvider.Services
{
    public class CertificateService : ICertificateService
    {
        public X509Certificate2 GetCertificate(string issuer)
        {
            X509Store computerCaStore = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            X509Certificate2 cert = null;

            try
            {
                computerCaStore.Open(OpenFlags.MaxAllowed);
                X509Certificate2Collection certificatesInStore = computerCaStore.Certificates;

                foreach (X509Certificate2 c in certificatesInStore)
                {
                    if (c.Issuer == issuer)
                    {
                        cert = c;
                        break;
                    }
                }
            }
            catch
            {
                Trace.TraceError("Certificate error.");
            }

            return cert;
        }
    }
}
