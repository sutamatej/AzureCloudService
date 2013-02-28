using Microsoft.WindowsAzure.Storage.Queue;

namespace BackgroundWorker.Application
{
    public interface IQueueMessageParser
    {
        void Parse(CloudQueueMessage message);
    }
}
