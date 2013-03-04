using System.Threading;

namespace BackgroundWorker.Application.Services
{
    public class SleepService : ISleepService
    {
        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}