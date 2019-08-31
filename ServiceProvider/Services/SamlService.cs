﻿using ServiceProvider.Models.SAML;
using System;

namespace ServiceProvider.Services
{
    public interface ISamlService
    {
        AuthnRequest GetSamlRequest();
    }

    public class SamlService
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
                Destination = "dest",
                ID = "sadfadfsadfsdf",
                AssertionConsumerServiceURL = "consumerurl.com/?consuume",
                IssueInstant = DateTime.Now,
                Issuer = "issuer",
                ProviderName = "provider name",
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
