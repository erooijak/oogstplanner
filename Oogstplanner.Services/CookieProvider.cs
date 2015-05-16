using System;
using System.Web;

namespace Oogstplanner.Services
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

        public void RemoveCookie(string key)
        {
            /* Removing a cookie is not possible, so overwrite cookie to expire yesterday. */
            var value = HttpContext.Current.Request.Cookies[key].Value;
            SetCookie(key, value, -1);
        }
    }
}
