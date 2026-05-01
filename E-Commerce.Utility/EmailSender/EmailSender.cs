using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace BooksWeb.Utility.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var settings = _config.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(settings["SenderName"], settings["SenderEmail"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            var builder = new BodyBuilder { HtmlBody = body };
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            // Connect to MailHog
            await client.ConnectAsync(
                settings["SmtpServer"],
                int.Parse(settings["SmtpPort"]!),
                SecureSocketOptions.None // MailHog doesn't use SSL/TLS
            );
            if (!string.IsNullOrEmpty(settings["Username"]))
            {
                await client.AuthenticateAsync(settings["Username"], settings["Password"]);
            }
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
