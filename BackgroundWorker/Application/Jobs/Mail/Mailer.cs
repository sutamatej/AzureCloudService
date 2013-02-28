using ActionMailer.Net.Standalone;
using BackgroundWorker.Application.Jobs.Mail.Models;

namespace BackgroundWorker.Application.Jobs.Mail
{
    public class Mailer : RazorMailerBase, IMailer
    {
        private readonly string _viewPath;

        public override string ViewPath
        {
            get { return _viewPath; }
        }

        public Mailer(string viewPath)
        {
            _viewPath = viewPath;
        }

        public RazorEmailResult Welcome()
        {
            From = "me@me.com";
            To.Add("test@test.com");
            Subject = "Test";

            // looks like ActionMailer.Net.Standalone requires specifically RazorEngine version 3.0.8
            return Email("Welcome");
        }

        public RazorEmailResult Goodbye(ApprovalEmailModel model)
        {
            From = "john@john.com";
            To.Add("asdf@asdf.com");
            Subject = "Typed test";
            return Email("Goodbye", model);
        }
    }
}
