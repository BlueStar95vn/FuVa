using FuturifyVacation.Data;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;
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
        private ApplicationDbContext _db;

        public EmailSender(ApplicationDbContext db)
        {
            _db = db;
        }

        public  Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("vt.sendemail@gmail.com", "Abc1234@"),
                EnableSsl = true
            };
            client.Send("vt.sendemail@gmail.com", email, subject, message);          
            return Task.CompletedTask;
        }

        public string GetEmailAdmin()
        {
            string allEmail = "";
            var getEmail = _db.UserProfiles.Include(u => u.User).Where(u => u.Position == "ADMIN").Select(u => u.User.Email).ToArray();
            foreach (string email in getEmail)
            {
                allEmail = allEmail + "," + email;
            }
            return allEmail;
        }
    }
}
