using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DotNetOpenAuth.FacebookOAuth2;
using Microsoft.Web.WebPages.OAuth;
using DotNetOpenAuth.AspNet;
using System;
using Newtonsoft.Json;

namespace Zk
{
    static class AuthConfig
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
           
            OAuthWebSecurity.RegisterClient(new FacebookScopedClient("379766855533323", "5a067c318bc027d61a3eda51c6b86c25"), "Facebook", null);

            //OAuthWebSecurity.RegisterGoogleClient();
            //var client = new GoogleOAuth2Client("664967518482-ungm4iiv4hufbfkdir6uueqh7ueubpl2.apps.googleusercontent.com", "9ofXNqX_fu4u20ly8L5UXsDR");
            //var extraData = new Dictionary<string, object>();
            //OAuthWebSecurity.RegisterClient(client, "Google", extraData);
        }
    }

    public class FacebookScopedClient : IAuthenticationClient
    {
        private string appId;
        private string appSecret;

        private const string baseUrl = "https://www.facebook.com/dialog/oauth?client_id=";
        public const string graphApiToken = "https://graph.facebook.com/oauth/access_token?";
        public const string graphApiMe = "https://graph.facebook.com/me?";


        private static string GetHTML(string URL)
        {
            string connectionString = URL;

            try
            {
                System.Net.HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(connectionString);
                myRequest.Credentials = CredentialCache.DefaultCredentials;

                // Get the response
                WebResponse webResponse = myRequest.GetResponse();
                Stream respStream = webResponse.GetResponseStream();

                StreamReader ioStream = new StreamReader(respStream);
                string pageContent = ioStream.ReadToEnd();
                // Close streams
                ioStream.Close();
                respStream.Close();
                return pageContent;
            }
            catch (Exception)
            {
            }
            return null;
        }

        private  IDictionary<string, string> GetUserData(string accessCode, string redirectURI)
        {

            string token = GetHTML(graphApiToken + "client_id=" + appId + "&redirect_uri=" + HttpUtility.UrlEncode(redirectURI) + "&client_secret=" + appSecret + "&code=" + accessCode);
            if (token == null || token == "")
            {
                return null;
            }
            string data = GetHTML(graphApiMe + "fields=id,name,email,gender,link&access_token=" + Substring(token, "access_token=", "&"));

            // this dictionary must contains
            Dictionary<string, string> userData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            return userData;
        }

        public FacebookScopedClient(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        public string ProviderName
        {
            get { return "Facebook"; }
        }

        public void RequestAuthentication(System.Web.HttpContextBase context, Uri returnUrl)
        {
            string url = baseUrl + appId + "&redirect_uri=" + HttpUtility.UrlEncode(returnUrl.ToString()) + "&scope=email";
            context.Response.Redirect(url);
        }

        public AuthenticationResult VerifyAuthentication(System.Web.HttpContextBase context)
        {
            string code = context.Request.QueryString["code"];

            string rawUrl = context.Request.Url.OriginalString;
            //From this we need to remove code portion
            rawUrl = Regex.Replace(rawUrl, "&code=[^&]*", "");

            IDictionary<string, string> userData = GetUserData(code, rawUrl);

            if (userData == null)
                return new AuthenticationResult(false, ProviderName, null, null, null);

            string id = userData["id"];
            string username = userData["email"];
            userData.Remove("id");
            userData.Remove("email");

            AuthenticationResult result = new AuthenticationResult(true, ProviderName, id, username, userData);
            return result;
        }

        public static string Substring(string str, string startString, string endString)
        {
            if (str.Contains(startString))
            {
                int iStart = str.IndexOf(startString, StringComparison.Ordinal) + startString.Length;
                int iEnd = str.IndexOf(endString, iStart, StringComparison.Ordinal);

                return str.Substring(iStart, (iEnd - iStart));
            }
            return null;
        }

    }
        
        

}
