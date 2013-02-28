using System.Threading;

namespace BackgroundWorker.Application.Services
{
    public class SleepService : ISleepService
    {
        public SleepService()
        {
        }

        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}