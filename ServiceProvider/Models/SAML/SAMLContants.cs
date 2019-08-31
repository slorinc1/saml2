namespace ServiceProvider.Models.SAML
{
    public class SAMLContants
    {
        public const string SAMLP_NAMESPACE = "urn:oasis:names:tc:SAML:2.0:protocol";
        public const string SAML_NAMESPACE = "urn:oasis:names:tc:SAML:2.0:assertion";
        public const string Version = "2.0";
        public const string ProtocolBinding = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST";
        public const string Format = "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress";
        public const string AuthnContextClassRef = "urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport";
    }
}
