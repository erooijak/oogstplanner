using Oogstplanner.Models;

namespace Oogstplanner.Services
{
    public interface IAuthenticationService
    {
        AuthenticatedStatus GetAuthenticationStatus();
    }
}
    