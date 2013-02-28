using Microsoft.WindowsAzure.Storage.Queue;

namespace BackgroundWorker.Application
{
    public interface IQueue
    {
        CloudQueueMessage GetMessage();

        void AddMessage(CloudQueueMessage message);

        void DeleteMessage(CloudQueueMessage message);
    }
}
