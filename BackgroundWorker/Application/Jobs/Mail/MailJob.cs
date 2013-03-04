using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;

namespace BackgroundWorker.Application.Jobs.Mail
{
    public class MailJob : IJob
    {
        private readonly IMailer _mailer;
        private readonly IEmailDataProvider _emailDataProvider;
        private string emailType;

        public MailJob(IMailer mailer, IEmailDataProvider emailDataProvider)
        {
            _mailer = mailer;
            _emailDataProvider = emailDataProvider;
        }

        public void ParseJobMessage(string[] messageParts)
        {
            emailType = messageParts[1];
        }

        public void Execute()
        {
            switch (emailType)
            {
                case "welcome":
                    _mailer.Welcome().DeliverAsync();
                    break;
                case "goodbye":
                    var model = _emailDataProvider.GetGoodbyeEmailModel();
                    _mailer.Goodbye(model).DeliverAsync();
                    break;
                default:
                    throw new NotSupportedException(String.Format("The email type of {0} is not supported.", emailType));
            }
        }
    }
}
