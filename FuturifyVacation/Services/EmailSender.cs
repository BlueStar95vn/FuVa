﻿using FuturifyVacation.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FuturifyVacation.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("vt.sendemail@gmail.com", "Abc1234@"),
                EnableSsl = true
            };
            client.Send("vt.sendemail@gmail.com", email, subject, message);          
            return Task.CompletedTask;
        }
    }
}
