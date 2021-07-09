using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MyFirstEShop.Repositories
{
    public interface IMessageSenderRepository
    {
        public Task SendEmail(string Email, string Subject, string MessageBody, bool IsHtml = false);
    }

    public class MessageSenderRepository : IMessageSenderRepository
    {
        public Task SendEmail(string Email, string Subject, string MessageBody, bool IsHtml = false)
        {
            using (var Client = new SmtpClient())
            {

                var EmailMessage = new MailMessage(new MailAddress("SiteGmail@gmail.com"), new MailAddress(Email));

                EmailMessage.To.Add(new MailAddress(Email)); 
                EmailMessage.From = new MailAddress("SiteGmail@gmail.com"); 
                EmailMessage.Subject = Subject;
                EmailMessage.Body = MessageBody;
                EmailMessage.IsBodyHtml = IsHtml; 

                var Credential = new NetworkCredential()
                {
                    UserName = "SiteGmail@gmail.com", 
                    Password = "Site Password"
                };

                Client.Host = "smtp.gmail.com";
                Client.Port = 587;
                Client.EnableSsl = true;
                Client.UseDefaultCredentials = true;
                Client.Credentials = Credential;


                Client.Send(EmailMessage);
            }

            return Task.CompletedTask;

        }
    }
}