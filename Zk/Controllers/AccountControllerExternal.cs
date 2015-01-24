using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

using Zk.Models;

namespace Zk.Controllers
{
    [Authorize]
    public partial class AccountController : Controller
    {

        //
        // GET: /Account/ExternalLogin
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, "http://localhost:8080/Account/ExternalLoginCallback?returnUrl=" + returnUrl);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            //DotNetOpenAuth.FacebookOAuth2.FacebookOAuth2Client.RewriteRequest();
            var result = OAuthWebSecurity.VerifyAuthentication("http://localhost:8080/Account/ExternalLoginCallback?returnUrl=" + returnUrl);


            // check if logon at Facebook was successful
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            // check if user profile/model is known and Enabled
            var username = OAuthWebSecurity.GetUserName(result.Provider, result.ProviderUserId);
            if (username != null && WebSecurity.GetUserId(username) > 0 
                && _manager.GetUserById(WebSecurity.GetUserId(username)).Enabled == false)
            {
                ModelState.AddModelError("registration", "Deze account is uitgeschakeld.");
                return View("Login");
            }

            // try to login this user locally
            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            // login failed, thus user not yet registered

            // User is new and qualified; create a userprofile
            return this.ExternalLoginCreateUserProfile(result, returnUrl);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
            
        private ActionResult ExternalLoginCreateUserProfile(AuthenticationResult result, string returnUrl)
        {
        
            var UserName = result.ExtraData["email"].ToLower();
            var FullName = result.ExtraData["name"];
            var Email = result.ExtraData["email"];

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                _manager.AddUser(UserName,FullName, Email);

                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, UserName);

                // try to login locally
                if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
                {
                    return RedirectToLocal(returnUrl);
                }

            }

            // something went wrong; back to login screen
            return View("Login");
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

    }
}