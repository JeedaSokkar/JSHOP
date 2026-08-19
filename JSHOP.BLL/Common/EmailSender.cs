using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.BLL.Common
{
    public class EmailSender : IEmailSender
    {
        
       public Task SendEmailAsync(string email, string subject, string message)
           {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("jeedasokkar2003@gmail.com", "tytl obaa mlcx bvgw")
            };

            return client.SendMailAsync(
                new MailMessage(from: "jeedasokkar2003@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {IsBodyHtml = true});
        }
    }

}

