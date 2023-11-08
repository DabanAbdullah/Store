
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Store.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {//logic here to send email;


            // Configure the Gmail SMTP server and port
            var smtpServer = "smtp.gmail.com";
            var smtpPort = 587;

            // Create a new SMTP client
            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                // Configure the client with your Gmail credentials
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("dabanabdullah.de@gmail.com", "rwnl chpn coet tgwf");

                // Enable SSL encryption
                client.EnableSsl = true;

                // Create a new email message
                var mail = new MailMessage();

                // Set the sender's email address
                mail.From = new MailAddress("dabanabdullah.de@gmail.com");

                // Set the recipient's email address
                mail.To.Add(email);

                // Set the subject and body of the email
                mail.Subject = subject;
                mail.Body = htmlMessage;
                mail.IsBodyHtml= true;

                // You can also add attachments if needed
                // var attachment = new Attachment("path_to_file.pdf", MediaTypeNames.Application.Pdf);
                // mail.Attachments.Add(attachment);

                // Send the email
                try
                {
                    client.Send(mail);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                }




                
            }
            return Task.CompletedTask;
        }
    }
}
