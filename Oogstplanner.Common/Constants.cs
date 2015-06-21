using System;
using System.Configuration;
using System.Net.Configuration;

namespace Oogstplanner.Common
{
    public static class Constants
    {
        public static string AnonymousUserCookieKey
        {
            get 
            {
                return ConfigurationManager.AppSettings["AnonymousUserCookieKey"];
            }
        }

        public static double AnonymousUserCookieExpiration
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings["AnonymousUserCookieExpiration"]);
            }
        }

        public static string ConnectionStringName
        {
            get
            {
                #if DEBUG
                return "TestOogstplannerDatabaseConnection";
                #else
                return "ProductionOogstplannerDatabaseConnection";
                #endif
            }
        }
    }
}
