using System.Net.Mail;
using System.Net.Security;

using Oogstplanner.Common;
using Oogstplanner.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace Oogstplanner.Services
{
    public class GMailService : ServiceBase, IEmailService
    {
        public GMailService(IOogstplannerUnitOfWork unitOfWork) 
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
