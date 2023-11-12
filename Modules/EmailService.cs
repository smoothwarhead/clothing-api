

using KerryCoAdmin.Api.Entities.Models;
using KerryCoAdmin.Api.Interfaces;
using KerryCoAdmin.Api.Modules;
using KerryCoAdmin.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;

namespace Services
{
    public class EmailService : ControllerBase
    {

        public async static Task<Response> SendEmail(string email, string content, string plainText, string subj, EmailSettings emailSettings)
        {
            //var emailSettings = _configuration.GetSection("SendGrid").Get<EmailSettings>();
            //var key = "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";
            var client = new SendGridClient(emailSettings.Key);
            var from = new EmailAddress(emailSettings.SenderEmail, emailSettings.Note);
            var subject = subj;
            var to = new EmailAddress(email, emailSettings.Note);
            var plainTextContent = plainText;
            var htmlContent = content;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            //Console.WriteLine(response);



            return response;

        }

       

    }
}