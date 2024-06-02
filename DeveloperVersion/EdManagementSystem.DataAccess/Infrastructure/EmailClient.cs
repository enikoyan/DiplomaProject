using EdManagementSystem.DataAccess.Interfaces;
using System.Net.Mail;
using System.Net;
using EdManagementSystem.DataAccess.DTO;

namespace EdManagementSystem.DataAccess.Infrastructure
{
    public class EmailClient : IEmailClient
    {
        public bool SendMessage(EmailConfigDTO emailConfig, string targetAddress, string message)
        {
            string smtpServer = emailConfig.SmtpServer;
            int smtpPort = emailConfig.SmtpPort;
            string smtpUsername = emailConfig.SmtpUsername;
            string smtpPassword = emailConfig.SmtpPassword;

            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.IsBodyHtml = true;
                    mailMessage.From = new MailAddress(smtpUsername, "Система УМДО");
                    mailMessage.To.Add(targetAddress);
                    mailMessage.Subject = "УМДО - восстановление пароля";
                    mailMessage.Body = message;

                    try
                    {
                        smtpClient.Send(mailMessage);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
        }
    }
}