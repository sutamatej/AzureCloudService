using BackgroundWorker.Application.Jobs.Mail;
using BackgroundWorker.Application.Jobs.Mail.Models;
using System;
using System.IO;
using Xunit;

namespace Tests.BackgroundWorker.Jobs.Mail
{
    public class MailerTests
    {
        [Fact]
        public void Mailer_renders_welcome_email()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\BackgroundWorker\\EmailTemplates");
            var mailer = new Mailer(path);

            var email = mailer.Welcome();
            var body = new StreamReader(email.Mail.AlternateViews[0].ContentStream).ReadToEnd();

            Assert.Equal("Hello", body);
        }

        [Fact]
        public void Mailer_renders_goodbye_email()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\BackgroundWorker\\EmailTemplates");
            var mailer = new Mailer(path);

            var email = mailer.Goodbye(new GoodbyeEmailModel { UserName = "Test" });
            var body = new StreamReader(email.Mail.AlternateViews[0].ContentStream).ReadToEnd();

            Assert.Equal("Goodbye Test", body);
        }
    }
}
