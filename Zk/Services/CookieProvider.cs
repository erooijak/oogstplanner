using System;
using System.Web;

namespace Zk.Services
{
    public class CookieProvider
    {
        public void SetCookieValue(string key, string value, double expiration)
        {
                var cookie = new HttpCookie(key, value);
                cookie.Expires = DateTime.Now.AddDays(expiration);
                HttpContext.Current.Response.SetCookie(cookie);
        }

        public string GetCookieValue(string key)
        {
            return (HttpContext.Current.Request.Cookies[key] != null) 
                ? HttpContext.Current.Request.Cookies[key].Value 
                : "";
        }

    }
}
    