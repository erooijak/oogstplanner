using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;

using Oogstplanner.Services;
using Oogstplanner.Controllers;
using Oogstplanner.Models;
using Oogstplanner.Repositories;

namespace Oogstplanner.Tests
{
    [TestFixture]
    public class AccountControllerTest
    {
        AccountController controller;

        [TestFixtureSetUp]
        public void Setup()
        {
            var userServiceMock = new Mock<IUserService>();
            var membershipServiceMock = new Mock<IMembershipService>();
            var passwordRecoveryServiceMock = new Mock<IPasswordRecoveryService>();

            this.controller = new AccountController(
                userServiceMock.Object, 
                membershipServiceMock.Object, 
                passwordRecoveryServiceMock.Object);
        }
         
        [Test]
        public void Controllers_Account_LoginOrRegisterModal()
        {
            // ARRANGE

            // ACT

            // ASSERT
        }
    }
}