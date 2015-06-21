namespace Oogstplanner.Services
{
    public interface IEmailService
    {
        void SendEmail(string subject, string body, string receiver);
    }
}
