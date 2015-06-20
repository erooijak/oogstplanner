using System;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Oogstplanner.Common;
using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Web.Utilities.ExtensionMethods;
using Oogstplanner.Web.Utitilies.Filters;

namespace Oogstplanner.Web.Controllers
{
    [Authorize]
    public sealed class AccountController : Controller
    {
        readonly IDeletableUserService userService;
        readonly IMembershipService membershipService;
        readonly IPasswordRecoveryService passwordRecoveryService;

        public AccountController(
            IDeletableUserService userService,
            IMembershipService membershipService,
            IPasswordRecoveryService passwordRecoveryService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            if (membershipService == null)
            {
                throw new ArgumentNullException("membershipService");
            }
            if (passwordRecoveryService == null)
            {
                throw new ArgumentNullException("passwordRecoveryService");
            }

            this.userService = userService;
            this.membershipService = membershipService;
            this.passwordRecoveryService = passwordRecoveryService;
        }

        //
        // GET: /gebruiker/inloggenofregistreren
        [AllowAnonymous]
        public ActionResult LoginOrRegisterModal()
        {
            return PartialView(Url.View("_LoginModal", "Account"));
        }

        //
        // POST: /gebruiker/inloggen
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
        // POST: /gebruiker/registreren/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAjax]
        public ActionResult Register(RegisterModel model)
        {           
            if (ModelState.IsValid)
            {
                Oogstplanner.Models.ModelError modelError;
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
        // GET: /gebruiker/uitloggen
        public ActionResult LogOff()
        {
            membershipService.SignOut();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /gebruiker/verwijderen/
        [HttpPost]
        public JsonResult Remove()
        {   
            try 
            {
                var currentUserId = userService.GetCurrentUserId();
                var userName = userService.GetUser(currentUserId).Name;

                userService.RemoveUser(currentUserId);
                membershipService.RemoveUser(userName);
                membershipService.SignOut();

                return Json(new { success = true });
            }
            catch
            {
                // TODO: Implement logging
                return Json(new { success = false });
            }
        }

        //
        // POST: /gebruiker/wachtwoordvergeten
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
        // GET: /gebruiker/wachtwoordreset/{token}
        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            var model = new ResetPasswordModel() 
            {
                ReturnToken = token
            };

            return View(model);
        }

        //
        // POST: /gebruiker/wachtwoordreset
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
        // GET: /gebruiker
        [HttpGet]
        public ActionResult Info()
        {
            var id = userService.GetCurrentUserId();
            var currentUser = userService.GetUser(id);

            return View(currentUser);
        }

        //
        // GET: /gebruiker/{userName}
        [HttpGet]
        [AllowAnonymous]
        public ActionResult UserInfo() // string userInfo (Mono bug workaround)
        {
            // Work around to get userName parameter (Mono bug)
            string requestedPath = Request.AppRelativeCurrentExecutionFilePath.TrimEnd('/');
            int positionLastSlash = requestedPath.LastIndexOf('/') + 1;
            string userName = requestedPath.Substring(positionLastSlash, requestedPath.Length - positionLastSlash).TrimEnd('/');

            User user = null;
            try 
            {
                user = userService.GetUserByName(userName);
            }
            catch (UserNotFoundException)
            {
                ViewBag.Message = "404 Gebruiker niet gevonden";
                Response.StatusCode = 404;

                return View("UserDoesNotExist");
            }
            catch
            {
                // TODO implement logging of inner exception.
                throw new HttpException(500, "Er is iets fout gegaan.");
            }

            return View("Info", user);
        }
    }
}
