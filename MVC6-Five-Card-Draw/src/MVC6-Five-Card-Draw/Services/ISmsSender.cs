using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC6_Five_Card_Draw.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
