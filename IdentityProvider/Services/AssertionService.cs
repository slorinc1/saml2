using SSOLibrary;
using System;

namespace IdentityProvider.Services
{
    public class AssertionService: IAssertionService
    {
        readonly ISamlXMLSerializer xmlSerializer;

        public AssertionService(ISamlXMLSerializer xmlSerializer)
        {
            this.xmlSerializer = xmlSerializer;
        }

        public string GetAssertion(string requestXml)
        {
            var request = this.xmlSerializer.Deserialize<AuthnRequest>(requestXml);
            var assertion = CreateAssertion(request.ID);
            var result = this.xmlSerializer.Serialize(assertion);

            return result;
        }

        public UnsignedSAMLResponse CreateAssertion(string id)
        {
            var attributeStatement = new AttributeStatement()
            {
                new SSOLibrary.Attribute()
                {
                    AttributeValue = new AttributeValue()
                    {
                        Value = "slorinc@email.com",
                        Type = "asdasd"
                    },
                    Name = "email",
                    NameFormat = "string"
                },

                new SSOLibrary.Attribute()
                {
                    AttributeValue = new AttributeValue()
                    {
                        Value = "Lőrinc Sándor",
                        Type = "name"
                    },
                    Name = "name",
                    NameFormat = "string"
                }
            };

            var response = new UnsignedSAMLResponse()
            {
                Destination = "dest",
                ID = Guid.NewGuid().ToString(),
                InResponseTo = id,
                IssueInstant = DateTime.Now,
                Version = SAMLContants.Version,
                Status = new Status()
                {
                    StatusCode = new StatusCode()
                    {
                        Value = "alma"
                    }
                },
                Issuer = "asdasd",
                Assertion = new Assertion()
                {
                    AuthnStatement = new AuthnStatement()
                    {
                        AuthnContext = new AuthnContext()
                        {
                            AuthnContextClassRef = "asda"
                        },
                        AuthnInstant = DateTime.Now,
                        SessionIndex = "asdasd",
                        SessionNotOnOrAfter = DateTime.Now.AddDays(12)
                    },
                    ID = "adsasd",
                    IssueInstant = DateTime.Now,
                    Issuer = "asdasd",
                    Subject = new Subject()
                    {
                        NameID = new NameID()
                        {
                            Format = "format",
                            SPNameQualifier = "spname",
                            Value = "value"
                        },
                        SubjectConfirmation = new SubjectConfirmation()
                        {
                            Method = "method",
                            SubjectConfirmationData = new SubjectConfirmationData()
                            {
                                InResponseTo = id,
                                NotOnOrAfter = DateTime.Now,
                                Recipient = "recipient"
                            }
                        }
                    },
                    Version = SAMLContants.Version,
                    Conditions = new Conditions()
                    {
                        AudienceRestriction = new AudienceRestriction()
                        {
                            Audience = "audience"
                        },
                        NotBefore = DateTime.MaxValue,
                        NotOnOrAfter = DateTime.MinValue
                    },
                    AttributeStatement = attributeStatement
                }
            };

            return response;
        }
    }
}
