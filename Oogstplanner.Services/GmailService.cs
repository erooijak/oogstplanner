using System.Net;
using System.Net.Mail;

using Oogstplanner.Data;

namespace Oogstplanner.Services
{
    public class GmailService : ServiceBase, IEmailService
    {
        public GmailService(IOogstplannerUnitOfWork unitOfWork) 
            : base(unitOfWork)
        { }
            
        public void SendEmail(string subject, string body, string receiver)
        {
            var mail = new MailMessage
                {
                    Subject = subject,
                    Body = body
                };
            mail.To.Add(receiver);

            using (var client = new SmtpClient())
            {
                // Note: apparently this (ignoring certificate check) is not a smart thing to do.
                //       But it seems the only way to get e-mailing via gmail to work.
                ServicePointManager.ServerCertificateValidationCallback += (s, crt, chain, sll) => true;

                client.Send(mail);
            }
        }
    }
}
