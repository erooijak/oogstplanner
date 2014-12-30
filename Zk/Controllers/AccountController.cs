using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

using Zk.BusinessLogic;
using Zk.Models;
using Zk.ViewModels;


namespace Zk.Controllers
{
    [Authorize]
    public partial class AccountController : Controller
    {
        private UserManager _manager;

        public AccountController()
        {
            _manager = new UserManager();
        }

        public AccountController(UserManager manager)
        {
            _manager = manager;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginOrRegisterViewModel viewModel, string returnUrl)
        {
            var model = viewModel.Login;

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // If we got this far, something failed, redisplay form
                    ModelState.AddModelError("", "De gebruikersnaam of het wachtwoord is incorrect.");
                }
            }
                
            return View(viewModel);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register/
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(LoginOrRegisterViewModel viewModel)
        {           
            var model = viewModel.Register;

            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;

                (Membership.Provider).CreateUser(
                    model.UserName, model.Password, model.Email, null, null, true, null, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    _manager.AddUser(model.UserName, model.FullName, model.Email);
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("registration", ErrorCodeToString(status));
                }
            }

            return View("Register", viewModel);
        }
       
        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/LostPassword
        [AllowAnonymous]
        public ActionResult LostPassword()
        {
            return View();
        }

        //
        // POST: Account/LostPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(LostPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _manager.GetMembershipUserByEmail(model.Email);

                if (user != null)
                {
                    // Generate token that will be used in the email link to authenticate user
                    var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                    // Generate the html link sent via email
                    var resetLink = "<a href='"
                        + Url.Action("ResetPassword", "Account", new { rt = token }, "http")
                        + "'>klik hier om uw Zaaikalender wachtwoord te resetten.</a>";

                    // Email stuff
                    var subject = "Reset uw wachtwoord voor de Zaaikalender";
                    var body = "Uw link: " + resetLink;
                    var from = "donotreply@zaaikalender.org";

                    var message = new MailMessage(from, model.Email) 
                    {
                        Subject = subject,
                        Body = body
                    };
                            
                    // Attempt to send the email and store the token.
                    using (var client = new SmtpClient()) 
                    {
                        try 
                        {
                            client.Send(message);
                            _manager.StoreResetToken(model.Email, token);
                        }
                        catch (Exception e) 
                        {
                            ModelState.AddModelError("", "Er is een probleem opgetreden bij het verzenden van de e-mail: " + e.Message);
                            return View(model);
                        }
                    }

                }

            }

            /* You may want to send the user to a "Success" page upon the successful
            * sending of the reset email link. Right now, if we are 100% successful
            * nothing happens on the page. :P
            */
            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {
            var model = new ResetPasswordModel() {
                ReturnToken = rt
            };

            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _manager.GetMembershipUserFromToken(model.ReturnToken);

                var isChangeSuccess = user.ChangePassword(
                    user.ResetPassword(),
                    model.Password
                );

                ViewBag.Message = isChangeSuccess 
                    ? "Wachtwoord succesvol veranderd." 
                    : "Er is iets hopeloos fout gegaan!";
            }

            return View(model);
        }

        //
        // GET: /Account
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {

            var statusMessage = new Dictionary<ManageMessageId?, String>()
            {
                { ManageMessageId.ChangePasswordSuccess, "Uw wachtwoord is veranderd." },
                { ManageMessageId.SetPasswordSuccess, "Uw wachtwoord is ingesteld." },
                { ManageMessageId.RemoveLoginSuccess, "De externe login is verwijderd." }
            };
            ViewBag.StatusMessage = (null != message && statusMessage.ContainsKey(message)) ? statusMessage[message] : "";

            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");

            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {

            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");

            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Het huidige wachtwoord is incorrect of het nieuwe wachtwoord is ongeldig.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", string.Format("Het is niet mogelijk om een lokaal gebruikersprofiel te creëren. Een profiel met de naam \"{0}\" bestaat mogelijk al.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        [ChildActionOnly]
        public PartialViewResult _UserInfo()
        {
            var model = new UserInfoViewModel();

            model.IsAuthenticated = User.Identity.IsAuthenticated;

            if (model.IsAuthenticated)
            {
                // Retrieve the name
                model.FullName = _manager.GetUser(User).FullName;

                // Return populated ViewModel
                return this.PartialView(model);
            }

            // Return the model with IsAuthenticated only
            return this.PartialView(model);
        }


        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.

            var errorcodes = new Dictionary<MembershipCreateStatus, String>()
            {
                { MembershipCreateStatus.DuplicateUserName, 
                    "Gebruikersnaam bestaat al. Vult u alstublieft een andere gebruikersnaam in." },
                { MembershipCreateStatus.DuplicateEmail, 
                    "Er bestaat al een gebruiker met dit e-mailadres. Vult u alstublieft een ander e-mailadres in." },
                { MembershipCreateStatus.InvalidPassword, 
                    "Het wachtwoord is niet geldig. Vult u alstublieft een geldig wachtwoord in." },
                { MembershipCreateStatus.InvalidEmail, 
                    "Het verstrekte e-mailadres is ongeldig. Controleert u alstublieft het adres en vul een correcte in." },
                { MembershipCreateStatus.InvalidAnswer, 
                    "Het antwoord op de geheime vraag is ongeldig. Controleert u alstublieft de waarde en probeer het opnieuw." },
                { MembershipCreateStatus.InvalidQuestion, 
                    "De geheime vraag is ongeldig. Controleert u alstublieft de waarde en probeer het opnieuw." },
                { MembershipCreateStatus.InvalidUserName, 
                    "De verstrekte gebruikersnaam is ongeldig. Controleert u alstublieft de waarde en probeer het opnieuw." },
                { MembershipCreateStatus.ProviderError, 
                    "De verstrekte authenticatie leidde tot een fout. Controleert u alstublieft de waarde en probeer het opnieuw. Als het probleem aanhoudt contacteer dan uw systeembeheerder." },
                { MembershipCreateStatus.UserRejected, 
                    "Het creëeren van een nieuwe gebruiker is mislukt. Controleert u alstublieft de waarde en probeer het opnieuw. Als het probleem aanhoudt contacteer dan uw systeembeheerder." }
            };

            if (errorcodes.ContainsKey(createStatus))
            {
                return errorcodes[createStatus];
            }
            else
            {
                return "Een onbekende fout heeft plaatsgevonden. Controleert u alstublieft de waarde en probeer het opnieuw. Als het probleem aanhoudt contacteer dan uw systeembeheerder.";
            }
        }
        #endregion
    }
}