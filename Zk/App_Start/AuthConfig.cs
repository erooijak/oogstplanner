using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;
using DotNetOpenAuth.GoogleOAuth2;

namespace Zk
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "379766855533323",
            //    appSecret: "5a067c318bc027d61a3eda51c6b86c25");
           
            //OAuthWebSecurity.RegisterGoogleClient();
            var client = new GoogleOAuth2Client("664967518482-ungm4iiv4hufbfkdir6uueqh7ueubpl2.apps.googleusercontent.com", "9ofXNqX_fu4u20ly8L5UXsDR");
            var extraData = new Dictionary<string, object>();
            OAuthWebSecurity.RegisterClient(client, "Google", extraData);
        }
    }
}
