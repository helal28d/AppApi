using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppApi.DTOs;

namespace AppApi.Services;

public interface IEmailService
{
    void SendEmail(EmailDto request);
    void SendCodeEmail(string to, string sub, string body);

}
