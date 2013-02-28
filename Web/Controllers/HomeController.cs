using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private CloudQueue jobQueue;

        public HomeController()
        {
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));
            var jobQueueClient = storageAccount.CreateCloudQueueClient();
            jobQueue = jobQueueClient.GetQueueReference("jobqueue");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            var message = new CloudQueueMessage("email;welcome");
            jobQueue.AddMessage(message);
            return View("Index");
        }

        public ActionResult Goodbye()
        {
            var message = new CloudQueueMessage("email;goodbye");
            jobQueue.AddMessage(message);
            return View("Index");
        }
    }
}
