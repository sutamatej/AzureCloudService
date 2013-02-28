using BackgroundWorker.Application.Jobs;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Collections.Generic;

namespace BackgroundWorker.Application
{
    public class QueueMessageParser : IQueueMessageParser
    {
        private readonly IDictionary<string, IJob> _jobs;

        public QueueMessageParser(IDictionary<string, IJob> jobs)
        {
            _jobs = jobs;
        }
        
        public void Parse(CloudQueueMessage message)
        {
            var messageParts = message.AsString.Split(new char[] { ';' });
            var jobType = messageParts[0];
            var job = _jobs[jobType];
            job.ParseJobMessage(messageParts);
            job.Execute();
        }
    }
}
