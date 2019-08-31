using System;
using System.Collections.Generic;
using System.Text;

namespace SSOLibrary
{
    public interface ISamlService
    {
        AuthnRequest GetSamlRequest();
    }

    public class SamlService : ISamlService
    {
        public SamlService()
        {
        }

        public AuthnRequest GetSamlRequest()
        {
            return new AuthnRequest()
            {
                Version = SAMLContants.Version,
                ProtocolBinding = SAMLContants.ProtocolBinding,
                Destination = "http://idp.example.com/SSOService.php",
                ID = $"{Guid.NewGuid().ToString().Replace("-","")}{DateTime.Now.Ticks}",
                AssertionConsumerServiceURL = "http://sp.example.com/demo1/index.php?acs",
                IssueInstant = DateTime.Now,
                Issuer = "Service Provider",
                ProviderName = "Service provider",
                NameIDPolicy = new NameIDPolicy()
                {
                    AllowCreate = true,
                    Format = "exact"
                },
                RequestedAuthnContext = new RequestedAuthnContext()
                {
                    Comparison = "exact",
                    AuthnContextClassRef = SAMLContants.AuthnContextClassRef
                }
            };
        }
    }
}
