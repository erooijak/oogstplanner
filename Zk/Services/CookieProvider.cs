using System;
using System.Web;

namespace Zk.Services
{
    public class CookieProvider : ICookieProvider
    {
        public void SetCookie(string key, string value, double expiration)
        {
                var cookie = new HttpCookie(key, value);
                cookie.Expires = DateTime.Now.AddDays(expiration);
                HttpContext.Current.Response.SetCookie(cookie);
        }

        public string GetCookie(string key)
        {
            return (HttpContext.Current.Request.Cookies[key] != null) 
                ? HttpContext.Current.Request.Cookies[key].Value 
                : "";
        }

    }
}
    