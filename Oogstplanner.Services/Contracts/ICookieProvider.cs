namespace Oogstplanner.Services
{
    public interface ICookieProvider
    {
        string AnonymousUserCookieKey { get; }
        double AnonymousUserCookieExpiration { get; }

        void SetCookie(string key, string value, double expiration);
        string GetCookie(string key);
        void RemoveCookie(string key);
    }
}
