using System;
using System.Web.Security;
using System.Security.Principal;

using Zk.Models;
using Zk.Repositories;

namespace Zk.BusinessLogic
{
    public class UserManager
    {
        readonly Repository _repository;

        public UserManager()
        {
            _repository = new Repository();
        }

        public UserManager(Repository repository)
        {
            _repository = repository;
        }

        public void AddUser(string userName, string fullName, string email)
        {
            _repository.AddUser(userName, fullName, email);
            Roles.AddUserToRole(userName, "user");
        }

        public User GetUser(IPrincipal user)
        {
            return _repository.GetUser(user);
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

    }
}