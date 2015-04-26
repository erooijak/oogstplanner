using System;

using Zk.Models;
using Zk.Repositories;

namespace Zk.Services
{
    public class AnonymousUserService : IUserService
    {
        readonly Repository repository;

        public AnonymousUserService(Repository repository)
        {
            this.repository = repository;
        }

        public void AddUser(string userName, string fullName, string email)
        {
            throw new NotImplementedException();
        }

        public int GetCurrentUserId()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }
            
    }
}
    