using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

using Zk.Controllers;
using Zk.ViewModels;
using Zk.Services;

namespace Zk.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Controllers_Home_Index_MonthOrdering()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var passedViewModel = (MainViewModel)((ViewResult)controller.Index()).Model;

            // Assert
            var expectedMonthOrdering = new Stack<string>(new[] 
            {   
                "augustus", "mei",   "februari", "november",  
                "juli",     "april", "januari",  "oktober",      
                "juni",     "maart", "december", "september"       
            });

            Assert.AreEqual(expectedMonthOrdering, passedViewModel.MonthsOrdered,
                "The months should be ordered in the above way to have proper display.");
        }

    }
}