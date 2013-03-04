using System.Diagnostics;

namespace BackgroundWorker.Application.Services
{
    public class TraceService : ITraceService
    {
        public void TraceError(string error)
        {
            Trace.TraceError(error);
        }
    }
}
