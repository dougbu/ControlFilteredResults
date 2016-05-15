﻿using System.Threading.Tasks;

namespace ControlFilteredResults.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
