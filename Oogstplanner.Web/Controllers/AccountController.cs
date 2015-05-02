using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;

using Oogstplanner.Utilities.CustomExceptions;
using Oogstplanner.Services;
using Oogstplanner.Models;
using Oogstplanner.ViewModels;

namespace Oogstplanner.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        readonly UserService userService;
        readonly PasswordRecoveryService passwordRecoveryService;

        public AccountController(
            UserService userService, 
            PasswordRecoveryService passwordRecoveryService)
        {
            this.userService = userService;
            this.passwordRecoveryService = passwordRecoveryService;
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
        public ActionResult Login(LoginOrRegisterViewModel viewModel, string returnUrl)
        {
            var model = viewModel.Login;
            var user = model.UserName;
            var password = model.Password;

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(user, password)
                    || Membership.ValidateUser(Membership.GetUserNameByEmail(user), password))
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

            if ((model.UserName + model.Email + model.UserName + model.FullName).ToLower().Contains("jorrit"))
                throw new JorritNotAllowedException("Jorrit is not allowed on this website.");

            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;

                (Membership.Provider).CreateUser(
                    model.UserName, model.Password, model.Email, null, null, true, null, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    userService.AddUser(model.UserName, model.FullName, model.Email);
                    FormsAuthentication.SetAuthCookie(model.UserName, true);

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
            Session.Abandon();

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
        public ActionResult LostPassword(LostPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = passwordRecoveryService.GetMembershipUserByEmail(model.Email);

                if (user != null)
                {
                    // Generate token that will be used in the email link to authenticate user
                    var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                    // Generate the html link sent via email
                    var resetLink = Url.Action("ResetPassword", "Account", new { rt = token }, "http");

                    // Email stuff
                    var subject = "Reset uw wachtwoord voor de Oogstplanner";
                    var body = "Gebruik binnen 24 uur de volgende link om uw wachtwoord te resetten: " + resetLink;
                    var from = "donotreply@oogstplanner.nl";

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
                            passwordRecoveryService.StoreResetToken(model.Email, token);
                            ViewBag.Message = "Reset link is succesvol verzonden.";
                        }
                        catch (Exception e) 
                        {
                            ModelState.AddModelError("", "Er is een probleem opgetreden bij het verzenden van de e-mail: " + e.Message);
                            return View(model);
                        }
                    }

                }

            }
                
            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {
            var model = new ResetPasswordModel() 
            {
                ReturnToken = rt
            };

            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = passwordRecoveryService.GetMembershipUserFromToken(model.ReturnToken);

                var timeStamp = passwordRecoveryService.GetTokenTimeStamp(model.ReturnToken);
                var currentTime = DateTime.Now;

                if (timeStamp == null) 
                {
                    ViewBag.Message = "We hebben de aanvraagtijd van uw wachtwoord reset token niet kunnen vinden.";
                    return View(model); 
                }

                if (timeStamp.Value.AddHours(24) < currentTime) 
                {
                    ViewBag.Message = "Uw wachtwoord reset token is verlopen.";
                    return View(model); 
                }

                var isChangeSuccess = user != null && user.ChangePassword(
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
        // GET: /Account/Info
        [HttpGet]
        public ActionResult Info()
        {
            var id = userService.GetCurrentUserId();
            var currentUser = userService.GetUser(id);

            return View(currentUser);
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