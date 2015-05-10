using System;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;

using Oogstplanner.Utilities.ExtensionMethods;
using Oogstplanner.Utitilies.Filters;
using Oogstplanner.Services;
using Oogstplanner.Models;

namespace Oogstplanner.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        readonly IUserService userService;
        readonly IMembershipService membershipService;
        readonly IPasswordRecoveryService passwordRecoveryService;

        public AccountController(
            IUserService userService,
            IMembershipService membershipService,
            IPasswordRecoveryService passwordRecoveryService)
        {
            this.userService = userService;
            this.membershipService = membershipService;
            this.passwordRecoveryService = passwordRecoveryService;
        }

        //
        // GET: /Account/LoginOrRegisterModal
        [AllowAnonymous]
        public ActionResult LoginOrRegisterModal()
        {
            return PartialView(Url.View("_LoginModal", "Account"));
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAjax]
        public ActionResult Login(LoginModel model)
        {
            var userNameOrEmail = model.UserNameOrEmail;
            var password = model.Password;

            if (ModelState.IsValid)
            {
                if (membershipService.ValidateUser(userNameOrEmail, password))
                {
                    membershipService.SetAuthCookie(userNameOrEmail, model.RememberMe);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // If we got this far, something failed, redisplay form
                    ModelState.AddModelError("login", "De gebruikersnaam/e-mailadres of het wachtwoord is incorrect.");
                }
            }
                
            return new EmptyResult();
        }

        //
        // POST: /Account/Register/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAjax]
        public ActionResult Register(RegisterModel model)
        {           
            if (ModelState.IsValid)
            {
                Oogstplanner.Utilities.CustomClasses.ModelError modelError;
                if (membershipService.TryCreateUser(model.UserName, model.Password, model.Email, out modelError))
                {
                    userService.AddUser(model.UserName, model.FullName, model.Email);
                    membershipService.SetAuthCookie(model.UserName, false);
                    membershipService.AddUserToRole(model.UserName, "user");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(modelError.Field, modelError.Message);
                }
            }
             
            return new EmptyResult();
        }
       
        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            membershipService.SignOut();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: Account/LostPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAjax]
        public ActionResult LostPassword(LostPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = passwordRecoveryService.GetMembershipUserByEmail(model.Email);

                if (user != null)
                {
                    // Generate token that will be used in the email link to authenticate user
                    var token = passwordRecoveryService.GenerateToken();

                    // Generate the html link sent via email
                    var resetLink = Url.Action("ResetPassword", "Account", new { rt = token }, "http");

                    // Email stuff
                    const string subject = "Reset uw wachtwoord voor de Oogstplanner";
                    string body = "Gebruik binnen 24 uur de volgende link om uw wachtwoord te resetten: " + resetLink;
                    const string from = "donotreply@oogstplanner.nl";

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
                        }
                    }

                }

            }
                
            return new EmptyResult();
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
                    : "Er is iets fout gegaan!";
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

    }
}
