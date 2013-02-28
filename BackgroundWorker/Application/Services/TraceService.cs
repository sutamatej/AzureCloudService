using System.Diagnostics;

namespace BackgroundWorker.Application.Services
{
    public class TraceService : ITraceService
    {
        public TraceService()
        {

        }

        public void TraceError(string error)
        {
            Trace.TraceError(error);
        }
    }
}
