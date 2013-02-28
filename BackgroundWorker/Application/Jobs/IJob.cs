
namespace BackgroundWorker.Application.Jobs
{
    public interface IJob
    {
        void ParseJobMessage(string[] messageParts);

        void Execute();
    }
}
