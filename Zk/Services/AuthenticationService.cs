using System.Threading;

namespace Zk.Services
{
    public class AuthenticationService
    {
        public AuthenticatedStatus GetAuthenticationStatus()
        {
            return Thread.CurrentPrincipal.Identity.IsAuthenticated 
                ? AuthenticatedStatus.Authenticated
                : AuthenticatedStatus.Anonymous; 
        }
    }

    public enum AuthenticatedStatus
    {
        Anonymous,
        Authenticated
    }

}
    