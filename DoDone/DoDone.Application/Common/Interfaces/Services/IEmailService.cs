using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Application.Common.Interfaces.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

}
