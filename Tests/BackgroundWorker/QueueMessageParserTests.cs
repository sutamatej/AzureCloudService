using BackgroundWorker.Application;
using BackgroundWorker.Application.Jobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Tests.BackgroundWorker
{
    public class QueueMessageParserTests
    {
        [Fact]
        public void Parser_parses_queue_message_and_executes_appropriate_job()
        {
            var jobs = new Dictionary<string, IJob>();
            var job = new Mock<IJob>();
            jobs.Add("test", job.Object);
            var parser = new QueueMessageParser(jobs);

            var message = new CloudQueueMessage("test");

            parser.Parse(message);

            job.Verify(j => j.ParseJobMessage(new string[] { "test" }), Times.Once());
            job.Verify(j => j.Execute(), Times.Once());
        }

        [Fact]
        public void Parser_throws_an_error_if_particular_job_doesnt_exist()
        {
            var jobs = new Dictionary<string, IJob>();
            var job = new Mock<IJob>();
            jobs.Add("aaa", job.Object);
            var parser = new QueueMessageParser(jobs);

            var message = new CloudQueueMessage("bbb");

            Assert.Throws<KeyNotFoundException>(() =>
                {
                    parser.Parse(message);
                });
        }
    }
}
