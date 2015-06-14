using System;
using System.Configuration;
using System.Web;

namespace Oogstplanner.Services
{
    public class CookieProvider : ICookieProvider
    {
        string anonymousUserKey;
        public string AnonymousUserCookieKey
        {
            get
            {
                if (anonymousUserKey == null)
                {
                    anonymousUserKey = ConfigurationManager.AppSettings["AnonymousUserCookieKey"];
                }
                return anonymousUserKey;
            }
        }

        double? anonymousUserCookieExpiration;
        public double AnonymousUserCookieExpiration
        {
            get
            {
                if (anonymousUserCookieExpiration == null)
                {
                    anonymousUserCookieExpiration = Convert.ToDouble(ConfigurationManager.AppSettings["AnonymousUserCookieExpiration"]);
                }
                return (double)anonymousUserCookieExpiration;
            }
        }

        public void SetCookie(string key, string value, double expirationInDays)
        {
            var cookie = new HttpCookie(key, value);
            cookie.Expires = DateTime.Now.AddDays(expirationInDays);
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
