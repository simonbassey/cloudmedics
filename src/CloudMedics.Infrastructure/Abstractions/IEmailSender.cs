using System;
using System.Threading.Tasks;
using CloudMedics.Infrastructure.Models;

namespace CloudMedics.Infrastructure
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailMessage message);
    }
}
