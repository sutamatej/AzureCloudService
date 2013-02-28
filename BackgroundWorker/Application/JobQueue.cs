using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace BackgroundWorker.Application
{
    public class JobQueue : IQueue
    {
        private CloudQueue jobQueue;

        public JobQueue(CloudStorageAccount storageAccount)
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            jobQueue = queueClient.GetQueueReference("jobqueue");
            jobQueue.CreateIfNotExists();
        }

        public CloudQueueMessage GetMessage()
        {
            return jobQueue.GetMessage();
        }

        public void AddMessage(CloudQueueMessage message)
        {
            jobQueue.AddMessage(message);
        }

        public void DeleteMessage(CloudQueueMessage message)
        {
            jobQueue.DeleteMessage(message);
        }
    }
}