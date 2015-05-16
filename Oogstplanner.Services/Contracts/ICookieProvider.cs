namespace Oogstplanner.Services
{
    public interface ICookieProvider
    {
        void SetCookie(string key, string value, double expiration);
        string GetCookie(string key);
        void RemoveCookie(string key);
    }
}
    