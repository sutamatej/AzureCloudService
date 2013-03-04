using ActionMailer.Net;
using ActionMailer.Net.Standalone;
using BackgroundWorker.Application.Jobs.Mail;
using BackgroundWorker.Application.Jobs.Mail.Models;
using Moq;
using System;
using System.Net.Mail;
using Xunit;

namespace Tests.BackgroundWorker.Jobs.Mail
{
    public class MailJobTests
    {
        [Fact]
        public void Mail_job_parses_welcome_email_queue_message_and_sends_welcome_email()
        {
            var sender = new Mock<IMailSender>();
            var mailInterceptor = new Mock<IMailInterceptor>();
            var emailResult = new RazorEmailResult(mailInterceptor.Object, sender.Object, new MailMessage(), "test", null, "test");

            var mailer = new Mock<IMailer>();
            mailer.Setup(m => m.Welcome()).Returns(emailResult);
            var emailDataProvider = new Mock<IEmailDataProvider>();
            var mailJob = new MailJob(mailer.Object, emailDataProvider.Object);

            mailJob.ParseJobMessage(new string[] { "email", "welcome" });
            mailJob.Execute();

            mailer.Verify(m => m.Welcome(), Times.Once());
        }

        [Fact]
        public void Mail_job_parses_goodbye_email_queue_message_and_sends_goodbye_email()
        {
            var sender = new Mock<IMailSender>();
            var mailInterceptor = new Mock<IMailInterceptor>();
            var emailResult = new RazorEmailResult(mailInterceptor.Object, sender.Object, new MailMessage(), "test", null, "test");

            var mailer = new Mock<IMailer>();
            mailer.Setup(m => m.Goodbye(It.IsAny<GoodbyeEmailModel>())).Returns(emailResult);
            var emailDataProvider = new Mock<IEmailDataProvider>();
            var mailJob = new MailJob(mailer.Object, emailDataProvider.Object);

            mailJob.ParseJobMessage(new string[] { "email", "goodbye" });
            mailJob.Execute();

            emailDataProvider.Verify(e => e.GetGoodbyeEmailModel(), Times.Once());
            mailer.Verify(m => m.Goodbye(It.IsAny<GoodbyeEmailModel>()), Times.Once());
        }

        [Fact]
        public void Mail_job_throws_an_error_if_mail_type_is_unknown()
        {
            var mailer = new Mock<IMailer>();
            var emailDataProvider = new Mock<IEmailDataProvider>();
            var mailJob = new MailJob(mailer.Object, emailDataProvider.Object);

            mailJob.ParseJobMessage(new string[] { "email", "unknown" });

            Assert.Throws<NotSupportedException>(() =>
                {
                    mailJob.Execute();
                });
        }
    }
}
