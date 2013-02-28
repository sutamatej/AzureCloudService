using BackgroundWorker.Application.Jobs.Mail.Models;
using System;

namespace BackgroundWorker.Application.Jobs.Mail
{
    public class EmailDataProvider : IEmailDataProvider
    {
        public EmailDataProvider()
        {
        }

        // Do fancy data loading here

        public ApprovalEmailModel GetApprovalEmailModel()
        {
            return new ApprovalEmailModel
            {
                UserName = "James Bond",
                Date = DateTime.Today
            };
        }
    }
}
