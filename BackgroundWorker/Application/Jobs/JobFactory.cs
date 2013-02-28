using BackgroundWorker.Application.Jobs.Mail;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackgroundWorker.Application.Jobs
{
    public class JobFactory
    {
        public static IDictionary<string, IJob> Create()
        {
            var jobs = new Dictionary<string, IJob>();

            var executionPath = Environment.CurrentDirectory;
            var templatePath = Path.Combine(executionPath, "EmailTemplates");

            var emailDataProvider = new EmailDataProvider();
            jobs.Add("email", new MailJob(new Mailer(templatePath), emailDataProvider));

            return jobs;
        }
    }
}
