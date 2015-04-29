namespace Zk.Services
{
    public interface ICookieProvider
    {
        void SetCookie(string key, string value, double expiration);
        string GetCookie(string key);
    }
}
    