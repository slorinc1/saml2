using System;

namespace SSOLibrary
{
    public interface ISamlXMLSerializer
    {
        string Serialize(AuthnRequest authnRequest);

        string Serialize(UnsignedSAMLResponse samlResponse);

        string Serialize(Type type, object value);

        T Deserialize<T>(string xml);
    }
}
