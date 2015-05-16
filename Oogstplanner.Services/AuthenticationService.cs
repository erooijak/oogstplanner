using System.Threading;

using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticatedStatus GetAuthenticationStatus()
        {
            return Thread.CurrentPrincipal.Identity.IsAuthenticated 
                ? AuthenticatedStatus.Authenticated
                : AuthenticatedStatus.Anonymous; 
        }
    }
        
}
