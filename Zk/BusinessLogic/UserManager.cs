using System;
using System.Web;
using System.Web.Security;
using System.Security.Principal;

using Zk.Models;
using Zk.Repositories;

namespace Zk.BusinessLogic
{
    public class UserManager
    {
        readonly Repository _repository;

        public UserManager(Repository repository)
        {
            _repository = repository;
        }

        public void Add(string userName, string fullName, string email)
        {
            var user = new User 
            {
                Name = userName,
                FullName = fullName,
                Email = email,
                Enabled = true
            };
            _repository.AddUser(user);
            Roles.AddUserToRole(userName, "user");

            // Get the actual user from the database, so we get the created UserId.
            var newlyCreatedUser = _repository.GetUserByUserName(userName);

            // Create calendar for the user
            _repository.CreateCalendar(newlyCreatedUser);
        }

        public int GetCurrentUserId()
        {
            // Note: HttpContext.Current.User.Identity.Name returns Username locally, and e-mail address on Debian.
            //       Has to be investigated. For now the quick fix below. :/

            var currentUserEmailOrName = HttpContext.Current.User.Identity.IsAuthenticated 
                ? HttpContext.Current.User.Identity.Name
                : "";

            int currentUserId = currentUserEmailOrName.Contains("@")
                ? _repository.GetUserIdByEmail(currentUserEmailOrName)
                : _repository.GetUserIdByName(currentUserEmailOrName);
          
            return currentUserId;
        }

        public User GetUserById(int id)
        {
            return _repository.GetUserById(id);
        }

        public MembershipUser GetMembershipUserByEmail(string email)
        {
            return _repository.GetMembershipUserByEmail(email);
        }

        public void StoreResetToken(string email, string token)
        {
            var timeResetRequested = DateTime.Now;
            _repository.StoreResetToken(email, timeResetRequested, token);
        }

        public MembershipUser GetMembershipUserFromToken(string token)
        {
            return _repository.GetMembershipUserFromToken(token);
        }

        public DateTime? GetTokenTimeStamp(string token)
        {
            return _repository.GetTokenTimeStamp(token);
        }

    }
}