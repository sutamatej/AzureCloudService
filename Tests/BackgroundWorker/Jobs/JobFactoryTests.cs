using BackgroundWorker.Application.Jobs;
using BackgroundWorker.Application.Jobs.Mail;
using Xunit;

namespace Tests.BackgroundWorker.Jobs
{
    public class JobFactoryTests
    {
        [Fact]
        public void Job_factory_creates_email_job()
        {
            var jobs = JobFactory.Create();

            Assert.IsType<MailJob>(jobs["email"]);
        }
    }
}
