using System.Threading;

using Zk.Models;

namespace Zk.Services
{
    public class AuthenticationService
    {
        public AuthenticatedStatusEnum GetAuthenticationStatus()
        {
            return Thread.CurrentPrincipal.Identity.IsAuthenticated 
                ? AuthenticatedStatusEnum.Authenticated
                : AuthenticatedStatusEnum.Anonymous; 
        }
    }
}
    