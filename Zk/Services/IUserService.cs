using System;
using System.Web.Security;

using Zk.Models;

namespace Zk.Services
{
    public interface IUserService
    {
        void Add(string userName, string fullName, string email);

        int GetCurrentUserId();

        User GetUserById(int id);

        MembershipUser GetMembershipUserByEmail(string email);

    }
}
