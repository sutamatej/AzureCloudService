using BackgroundWorker.Application.Services;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Diagnostics;

namespace BackgroundWorker.Application
{
    public class QueueMessageProcessor
    {
        private readonly ISleepService _sleepService;
        private readonly IQueue _queue;
        private readonly ITraceService _traceService;
        private readonly IQueueMessageParser _messageParser;

        public QueueMessageProcessor(ISleepService sleepService, IQueue queue, ITraceService traceService, IQueueMessageParser messageParser)
        {
            _sleepService = sleepService;
            _queue = queue;
            _traceService = traceService;
            _messageParser = messageParser;
        }

        public void Run(CloudQueueMessage msg)
        {
            try
            {
                bool messageFound = false;

                msg = _queue.GetMessage();
                if (msg != null)
                {
                    if (msg.DequeueCount > 5)
                    {
                        _traceService.TraceError(String.Format("Deleting poison job message:    message {0}.", msg.AsString));
                        _queue.DeleteMessage(msg);
                    }
                    else
                    {
                        _messageParser.Parse(msg);
                        _queue.DeleteMessage(msg);
                        messageFound = true;
                    }
                }

                if (messageFound == false)
                {
                    _sleepService.Sleep(1000 * 60);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                if (ex.InnerException != null)
                {
                    err += " Inner Exception: " + ex.InnerException.Message;
                }
                if (msg != null)
                {
                    err += " Last queue message retrieved: " + msg.AsString;
                }
                _traceService.TraceError(err);
                _sleepService.Sleep(1000 * 60);
            }
        }
    }
}