using BackgroundWorker.Application;
using BackgroundWorker.Application.Jobs;
using BackgroundWorker.Application.Services;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Diagnostics;
using System.Net;

namespace BackgroundWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private volatile bool onStopCalled = false;
        private volatile bool returnedFromRunMethod = false;
        private QueueMessageProcessor _messageProcessor;

        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount;
            ConfigureDiagnostics();

            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            var jobs = JobFactory.Create();

            _messageProcessor = new QueueMessageProcessor(
                new SleepService(),
                new JobQueue(storageAccount),
                new TraceService(),
                new QueueMessageParser(jobs));

            return base.OnStart();
        }

        public override void OnStop()
        {
            onStopCalled = true;
            while (returnedFromRunMethod == false)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        public override void Run()
        {
            CloudQueueMessage msg = null;
            Trace.TraceInformation("BackgroundWorker start of Run()");
            while (true)
            {
                if (onStopCalled == true)
                {
                    Trace.TraceInformation("OnStop() called BackgroundWorker");
                    returnedFromRunMethod = true;
                    return;
                }

                _messageProcessor.Run(msg);
            }
        }

        private void ConfigureDiagnostics()
        {
            DiagnosticMonitorConfiguration config = DiagnosticMonitor.GetDefaultInitialConfiguration();
            config.ConfigurationChangePollInterval = TimeSpan.FromMinutes(1d);
            config.Logs.BufferQuotaInMB = 500;
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1d);

            DiagnosticMonitor.Start(
                "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString",
                config);
        }
    }
}
