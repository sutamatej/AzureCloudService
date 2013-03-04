using BackgroundWorker.Application.Jobs.Mail.Models;

namespace BackgroundWorker.Application.Jobs.Mail
{
    public interface IEmailDataProvider
    {
        GoodbyeEmailModel GetGoodbyeEmailModel();
    }
}
