using System;
using System.Web.Mvc;
using System.Web.Security;

using Moq;
using NUnit.Framework;

using Oogstplanner.Common;
using Oogstplanner.Models;
using Oogstplanner.Services;
using Oogstplanner.Tests.Lib;
using Oogstplanner.Web.Controllers;

namespace Oogstplanner.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTest
    {
        [Test]
        public void Controllers_Account_LoginOrRegisterModal()
        {
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);
                
            // ACT
            var actionResult = controller.LoginOrRegisterModal() as PartialViewResult;

            // ASSERT
            Assert.AreEqual("~/Views/Account/_LoginModal.cshtml", actionResult.ViewName,
                "LoginOrRegisterModal should return the _LoginModal.cshtml partial view.");
        }

        [Test]
        public void Controllers_Account_Login_Success()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            membershipServiceMock.Setup(mock => mock.ValidateUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            const string expectedUserNameOrPassword = "test@test.de";
            var model = new LoginModel
            {
                    UserNameOrEmail = expectedUserNameOrPassword,
                    Password = "123456",
                    RememberMe = true
            };

            // ACT
            var actionResult = controller.Login(model) as RedirectToRouteResult;

            // ASSERT
            membershipServiceMock
                .Verify(mock => 
                    mock.SetAuthCookie(expectedUserNameOrPassword, true), 
                    Times.Once(),
                    "An persistent auth cookie should be set.");
            Assert.AreEqual("Home/Index", 
                string.Format("{0}/{1}", actionResult.RouteValues["controller"], actionResult.RouteValues["action"]),
                "When model and user are valid a redirect to Home/Index should take place.");
        }

        [Test]
        public void Controllers_Account_Login_SuccessNoAuthCookie()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            membershipServiceMock.Setup(mock => mock.ValidateUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);
                
            const string expectedUserNameOrPassword = "test@test.de";
            var model = new LoginModel
                {
                    UserNameOrEmail = expectedUserNameOrPassword,
                    Password = "123456",
                    RememberMe = false
                };

            // ACT
            controller.Login(model);

            // ASSERT
            membershipServiceMock
                .Verify(mock => 
                    mock.SetAuthCookie(expectedUserNameOrPassword, false), 
                    Times.Once(),
                    "A non persistent auth cookie should not be set.");
        }

        [Test]
        public void Controllers_Account_Login_Failure()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            membershipServiceMock.Setup(mock => mock.ValidateUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            var model = new LoginModel();

            controller.ModelState.AddModelError("", "");

            // ACT
            var actionResult = controller.Login(model);

            // ASSERT
            Assert.IsInstanceOf(typeof(EmptyResult), actionResult,
                "When model state is invalid an empty result should be returned.");
        }

        [Test]
        public void Controllers_Account_Login_MembershipUserInvalid()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            membershipServiceMock.Setup(mock => mock.ValidateUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);
                
            var model = new LoginModel
                {
                    UserNameOrEmail = "test@test.de",
                    Password = "123456",
                    RememberMe = true
                };       

            // ACT
            controller.Login(model);

            // ASSERT
            Assert.AreEqual("De gebruikersnaam/e-mailadres of het wachtwoord is incorrect.",
                controller.ViewData.ModelState["login"].Errors[0].ErrorMessage,
                "When the user has entered valid information, but the credentials are not valid" +
                "an error message should be displayed.");
        }

        [Test]
        public void Controllers_Account_Register_Success()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var expectedModelError = 
                new Oogstplanner.Models.ModelError();
            
            membershipServiceMock.Setup(mock => 
                mock.TryCreateUser(
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    out expectedModelError))
                .Returns(true);

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            const string expectedUserName = "Piet";
            const string expectedFullName = "Piet Jansma";
            const string expectedEmail = "test@test.de";
            var model = new RegisterModel
                {
                    UserName = expectedUserName,
                    FullName = expectedFullName,
                    Email = expectedEmail
                };

            // ACT
            var actionResult = controller.Register(model) as RedirectToRouteResult;

            // ASSERT
            userServiceMock
                .Verify(mock => 
                    mock.AddUser(expectedUserName, expectedFullName, expectedEmail), 
                    Times.Once(),
                "The user should be added.");
            membershipServiceMock
                .Verify(mock => 
                    mock.SetAuthCookie(expectedUserName, false), 
                    Times.Once(),
                "A non persistent auth cookie should be set");
            membershipServiceMock
                .Verify(mock => 
                    mock.AddUserToRole(expectedUserName, "user"), 
                    Times.Once(),
                    "New user should be added to the role user.");
            Assert.AreEqual("Home/Index", 
                string.Format("{0}/{1}", actionResult.RouteValues["controller"], actionResult.RouteValues["action"]),
                "When model and user are valid a redirect to Home/Index should take place.");
        }

        [Test]
        public void Controllers_Account_Remove_Success()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            const int expectedUserId = 12;
            const string expectedUserName = "testttt";

            userServiceMock.Setup(mock =>
                mock.GetCurrentUserId())
                .Returns(expectedUserId);

            userServiceMock.Setup(mock =>
                mock.GetUser(expectedUserId))
                .Returns(new User { Name = expectedUserName });

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            // ACT
            var actionResult = controller.Remove();

            // ASSERT
            userServiceMock
                .Verify(mock => 
                    mock.RemoveUser(expectedUserId), 
                    Times.Once(),
                    "The user should be removed via the user service.");
            membershipServiceMock
                .Verify(mock => 
                    mock.RemoveUser(expectedUserName), 
                    Times.Once(),
                    "The user's should be removed from the membership.");
            membershipServiceMock
                .Verify(mock => 
                    mock.SignOut(), 
                    Times.Once(),
                    "The user should be signed out.");
            Assert.IsTrue(actionResult.Data.ToString().Contains("success = True"),
                "Success should be returned");
        }

        [Test]
        public void Controllers_Account_Remove_Failure()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            userServiceMock.Setup(mock =>
                mock.GetCurrentUserId())
                .Throws(new InsufficientMemoryException());

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            // ACT
            var actionResult = controller.Remove();

            // ASSERT
            Assert.IsTrue(actionResult.Data.ToString().Contains("success = False"),
                "If something fails the controller should return this.");
        }

        [Test]
        public void Controllers_Account_Register_Failure()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var controller = new AccountController(
                userServiceMock.Object,
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            var model = new RegisterModel();

            controller.ModelState.AddModelError("", "");

            // ACT
            var actionResult = controller.Register(model);

            // ASSERT
            Assert.IsInstanceOf(typeof(EmptyResult), actionResult,
                "When model state is invalid an empty result should be returned.");
        }

        [Test]
        public void Controllers_Account_Register_FailureUserCreation()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            const string expectedModelErrorField = "Password";
            const string expectedModelErrorMessage = "Wrong password";
            var expectedModelError = 
                new Oogstplanner.Models.ModelError
                {
                    Field = expectedModelErrorField,
                    Message = expectedModelErrorMessage
                };

        membershipServiceMock.Setup(mock => 
                mock.TryCreateUser(
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    out expectedModelError))
                .Returns(false);

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);  

            const string expectedUserName = "Piet";
            const string expectedFullName = "Piet Jansma";
            const string expectedEmail = "test@test.de";
            var model = new RegisterModel
                {
                    UserName = expectedUserName,
                    FullName = expectedFullName,
                    Email = expectedEmail
                };

            // ACT
            var actionResult = controller.Register(model);

            // ASSERT
            Assert.IsInstanceOf(typeof(EmptyResult), actionResult,
                "An empty result should be returned when model state is invalid.");
            Assert.AreEqual(
                expectedModelErrorMessage, 
                controller.ViewData.ModelState[expectedModelErrorField].Errors[0].ErrorMessage,
                "The error message retrieved from the user creation service should be returned.");
        }

        [Test]
        public void Controllers_Account_LogOff()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);
                
            controller.SetMockControllerContext();

            // ACT
            var actionResult = controller.LogOff() as RedirectToRouteResult;

            // ASSERT
            membershipServiceMock
                .Verify(mock => 
                    mock.SignOut(), 
                    Times.Once(),
                    "Sign out method should be called.");
            Assert.AreEqual("Home/Index", 
                string.Format("{0}/{1}", actionResult.RouteValues["controller"], actionResult.RouteValues["action"]),
                "When model and user are valid a redirect to Home/Index should take place.");
        }

        [Test]
        public void Controllers_Account_LostPassword_NonExistingEmail()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var controller = new AccountController(
                                 userServiceMock.Object, 
                                 membershipServiceMock.Object, 
                                 passwordRecoveryServiceMock.Object);
                
            const string expectedEmail = "test@test.de";
            var model = new LostPasswordModel
            {
                Email = expectedEmail
            };

            // ACT
            var actionResult = controller.LostPassword(model);

            // ASSERT
            Assert.IsInstanceOf(typeof(EmptyResult), actionResult,
                "When the membership user could not be retrieved an empty result" +
                "should be returned (so it cannot be checked who is member and whose not without" +
                "logging in).");
        }

        [Test]
        public void Controllers_Account_LostPassword_Failure()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var userMock = new Mock<MembershipUser>();
            userMock.Setup(u => u.ProviderUserKey).Returns(Guid.NewGuid());

            passwordRecoveryServiceMock.Setup(mock => 
                mock.GetMembershipUserByEmail(It.IsAny<string>()))
                .Returns(userMock.Object);

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            controller.SetMockControllerContext();

            const string expectedEmail = "test@test.de";
            var model = new LostPasswordModel
                {
                    Email = expectedEmail
                };

            // ACT
            controller.LostPassword(model);

            // ASSERT
            Assert.IsTrue( 
                (controller.ViewData.ModelState[""].Errors[0].ErrorMessage)
                .Contains("Er is een probleem opgetreden bij het verzenden van de e-mail: The SMTP host was not specified"),
                "When the sending of a message fails this should be added to model. " +
                "(It fails since no SMTP host is specified in the unit test project.)");
        }

        [Test]
        public void Controllers_Account_ResetPasswordView()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            var expectedReturnToken = Guid.NewGuid().ToString();
            var model = new ResetPasswordModel
                {
                    ReturnToken = expectedReturnToken
                };

            // ACT
            var actionResult = controller.ResetPassword(model) as ViewResult;

            // ASSERT
            var actualReturnToken = ((ResetPasswordModel)actionResult.Model).ReturnToken;
            Assert.AreEqual(expectedReturnToken, actualReturnToken,
                "The return token should be returned in the view result.");
        }

        [Test]
        public void Controllers_Account_ResetPasswordSuccess()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var returnToken = Guid.NewGuid().ToString();
            const string resettedPassword = "Random";
            const string newPassword = "P@ssw0rd";

            var userMock = new Mock<MembershipUser>();
            userMock.Setup(mock => mock.ProviderUserKey)
                .Returns(Guid.NewGuid());
            userMock.Setup(mock => mock.ResetPassword())
                .Returns(resettedPassword);
            userMock.Setup(mock => mock.ChangePassword(resettedPassword, newPassword))
                .Returns(true);

            passwordRecoveryServiceMock.Setup(mock => 
                mock.GetMembershipUserFromToken(returnToken))
                .Returns(userMock.Object);
            passwordRecoveryServiceMock.Setup(mock => 
                mock.GetTokenTimeStamp(returnToken))
                .Returns(DateTime.Now.AddHours(-23));

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);
                
            var model = new ResetPasswordModel
            {
                    Password = newPassword,
                    ReturnToken = returnToken
            };

            // ACT
            controller.ResetPassword(model);

            // ASSERT
            Assert.AreEqual(controller.ViewData["Message"], "Wachtwoord succesvol veranderd.",
                "The message that the password was changed succesfully should be displayed.");
        }

        [Test]
        public void Controllers_Account_ResetPasswordFailureTokenExpired()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var returnToken = Guid.NewGuid().ToString();
            const string resettedPassword = "Random";
            const string newPassword = "P@ssw0rd";

            var userMock = new Mock<MembershipUser>();
            userMock.Setup(mock => mock.ProviderUserKey)
                .Returns(Guid.NewGuid());
            userMock.Setup(mock => mock.ResetPassword())
                .Returns(resettedPassword);
            userMock.Setup(mock => mock.ChangePassword(resettedPassword, newPassword))
                .Returns(true);

            passwordRecoveryServiceMock.Setup(mock => 
                mock.GetMembershipUserFromToken(returnToken))
                .Returns(userMock.Object);
            passwordRecoveryServiceMock.Setup(mock => 
                mock.GetTokenTimeStamp(returnToken))
                .Returns(DateTime.Now.AddHours(-25));

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            var model = new ResetPasswordModel
                {
                    Password = newPassword,
                    ReturnToken = returnToken
                };

            // ACT
            controller.ResetPassword(model);

            // ASSERT
            Assert.AreEqual(controller.ViewData["Message"], 
                "Uw wachtwoord reset token is verlopen.",
                "The message that the return token was expired should be shown.");
        }

        [Test]
        public void Controllers_Account_ResetPasswordFailureTokenTimeStampNotFound()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var returnToken = Guid.NewGuid().ToString();
            const string resettedPassword = "Random";
            const string newPassword = "P@ssw0rd";

            var userMock = new Mock<MembershipUser>();
            userMock.Setup(mock => mock.ProviderUserKey)
                .Returns(Guid.NewGuid());
            userMock.Setup(mock => mock.ResetPassword())
                .Returns(resettedPassword);
            userMock.Setup(mock => mock.ChangePassword(resettedPassword, newPassword))
                .Returns(true);

            passwordRecoveryServiceMock.Setup(mock => 
                mock.GetMembershipUserFromToken(returnToken))
                .Returns(userMock.Object);
            passwordRecoveryServiceMock.Setup(mock => 
                mock.GetTokenTimeStamp(returnToken))
                .Returns((DateTime?)null);

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            var model = new ResetPasswordModel
                {
                    Password = newPassword,
                    ReturnToken = returnToken
                };

            // ACT
            controller.ResetPassword(model);

            // ASSERT
            Assert.AreEqual(controller.ViewData["Message"], 
                "We hebben de aanvraagtijd van uw wachtwoord reset token niet kunnen vinden.",
                "The message that the return token was not found should be shown.");
        }

        [Test]
        public void Controllers_Account_ResetPasswordFailureOnMembershipChange()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var returnToken = Guid.NewGuid().ToString();
            const string resettedPassword = "Random";
            const string newPassword = "P@ssw0rd";

            var userMock = new Mock<MembershipUser>();
            userMock.Setup(mock => mock.ProviderUserKey)
                .Returns(Guid.NewGuid());
            userMock.Setup(mock => mock.ResetPassword())
                .Returns(resettedPassword);
            userMock.Setup(mock => mock.ChangePassword(resettedPassword, newPassword))
                .Returns(false);

            passwordRecoveryServiceMock.Setup(mock => 
                mock.GetMembershipUserFromToken(returnToken))
                .Returns(userMock.Object);
            passwordRecoveryServiceMock.Setup(mock => 
                mock.GetTokenTimeStamp(returnToken))
                .Returns(DateTime.Now.AddHours(-23));

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            var model = new ResetPasswordModel
                {
                    Password = newPassword,
                    ReturnToken = returnToken
                };

            // ACT
            controller.ResetPassword(model);

            // ASSERT
            Assert.AreEqual(controller.ViewData["Message"], 
                "Er is iets fout gegaan!",
                "The message that something went wrong should be shown.");
        }

        [Test]
        public void Controllers_Account_Info()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            const int expectedUserId = 1;
            var expectedUser = new User { Id = expectedUserId };

            userServiceMock.Setup(mock => mock.GetCurrentUserId())
                .Returns(expectedUserId);
            userServiceMock.Setup(mock => mock.GetUser(expectedUserId))
                .Returns(expectedUser);
                
            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);
                
            // ACT
            var viewResult = controller.Info() as ViewResult;

            // ASSERT
            Assert.AreEqual(expectedUser, (User)viewResult.Model);
        }

        [Test]
        public void Controllers_Account_UserInfo()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            var expectedUser = new User();

            userServiceMock.Setup(mock => mock.GetUserByName(It.IsAny<string>()))
                .Returns(expectedUser);

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            controller.SetMockControllerContext();

            // ACT
            var viewResult = controller.UserInfo() as ViewResult;

            // ASSERT
            Assert.AreEqual("Info", viewResult.ViewName,
                "The info view should be returned when al goes well");
            Assert.AreEqual(expectedUser, (User)viewResult.Model,
                "... and the user should be passed to it.");
        }

        [Test]
        public void Controllers_Account_UserInfoNotFound()
        {
            // ARRANGE
            var userServiceMock = new Mock<IDeletableUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            userServiceMock.Setup(mock => mock.GetUserByName(It.IsAny<string>()))
                .Throws<UserNotFoundException>();

            var controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);

            controller.SetMockControllerContext();

            // ACT
            var viewResult = controller.UserInfo() as ViewResult;

            // ASSERT
            Assert.AreEqual("UserDoesNotExist", viewResult.ViewName,
                "The user does not exist view should be returned when the user cannot be found.");
            Assert.AreEqual(404, controller.Response.StatusCode,
                "... with a 404 status code.");
        }
    }
}
