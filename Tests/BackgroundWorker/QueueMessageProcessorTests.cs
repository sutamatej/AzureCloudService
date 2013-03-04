using BackgroundWorker.Application;
using BackgroundWorker.Application.Services;
using Microsoft.WindowsAzure.Storage.Queue;
using Moq;
using System;
using Xunit;

namespace Tests.BackgroundWorker
{
    public class QueueMessageProcessorTests
    {
        [Fact]
        public void Processor_gets_message_from_queue()
        {
            // arrange
            var message = new CloudQueueMessage("email;test");
            var queue = new Mock<IQueue>();
            queue.Setup(q => q.GetMessage()).Returns(message);

            var sleepService = new Mock<ISleepService>();
            var traceService = new Mock<ITraceService>();
            var parser = new Mock<IQueueMessageParser>();

            var processor = new QueueMessageProcessor(sleepService.Object, queue.Object, traceService.Object, parser.Object);

            // act
            processor.Run(message);

            // assert
            queue.Verify(q => q.GetMessage(), Times.Once());
            parser.Verify(p => p.Parse(message), Times.Once());
            queue.Verify(q => q.DeleteMessage(message), Times.Once());
        }

        [Fact]
        public void Processor_sleeps_for_one_minute_if_theres_no_message()
        {
            var queue = new Mock<IQueue>();
            CloudQueueMessage message = null;
            queue.Setup(q => q.GetMessage()).Returns(message);

            var sleepService = new Mock<ISleepService>();
            var traceService = new Mock<ITraceService>();
            var parser = new Mock<IQueueMessageParser>();

            var processor = new QueueMessageProcessor(sleepService.Object, queue.Object, traceService.Object, parser.Object);

            processor.Run(message);

            queue.Verify(q => q.GetMessage(), Times.Once());
            sleepService.Verify(s => s.Sleep(60000), Times.Once());
        }

        [Fact]
        public void Processor_traces_thrown_error_and_sleeps_for_one_minute()
        {
            var queue = new Mock<IQueue>();
            var message = new CloudQueueMessage("job;type");
            queue.Setup(q => q.GetMessage()).Returns(message);

            var sleepService = new Mock<ISleepService>();
            var traceService = new Mock<ITraceService>();
            var parser = new Mock<IQueueMessageParser>();
            parser.Setup(p => p.Parse(message)).Throws<NotImplementedException>();

            var processor = new QueueMessageProcessor(sleepService.Object, queue.Object, traceService.Object, parser.Object);

            processor.Run(message);

            traceService.Verify(t => t.TraceError(It.IsAny<string>()), Times.Once());
            sleepService.Verify(s => s.Sleep(60000), Times.Once());
        }
    }
}
