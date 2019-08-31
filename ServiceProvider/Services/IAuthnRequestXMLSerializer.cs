using ServiceProvider.Models.SAML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Services
{
    public interface IAuthnRequestXMLSerializer
    {
        string Serialize(AuthnRequest authnRequest);
    }
}
