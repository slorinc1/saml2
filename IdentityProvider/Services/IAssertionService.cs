namespace IdentityProvider.Services
{
    public interface IAssertionService
    {
        string GetAssertion(string requestXml);
    }
}
