using ActionMailer.Net.Standalone;
using BackgroundWorker.Application.Jobs.Mail.Models;

namespace BackgroundWorker.Application.Jobs.Mail
{
    public interface IMailer
    {
        RazorEmailResult Welcome();

        RazorEmailResult Goodbye(GoodbyeEmailModel model);
    }
}
