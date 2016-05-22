using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC6_Five_Card_Draw.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
