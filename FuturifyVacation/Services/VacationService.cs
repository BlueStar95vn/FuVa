using FuturifyVacation.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuturifyVacation.Models;
using FuturifyVacation.Data;
using FuturifyVacation.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using FuturifyVacation.Models.BindingModels;

namespace FuturifyVacation.Services
{
    public class VacationService : IVacationService
    {
        private ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;
        public VacationService(ApplicationDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public async Task<UserVacation> AddVacationAsync(UserVacationViewModel model, string userId)
        {

            var vacation = new UserVacation
            {               
                Title = model.Title,
                UserId = userId,
                Start = model.Start,
                End = model.End,
                Color = model.Color
            };
            await _db.UserVacations.AddAsync(vacation);
            await _db.SaveChangesAsync();
            var getUser = await _db.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);

            ////Send Email
            //var email = GetEmailAdmin();
            //var subject = "[Vacation Tracking] - " + getUser.FirstName + " " + getUser.LastName + " booked a vacation!";
            //var message = "\n\nVacation Request Detail: "
            //                + "\n\nReason: " + model.Title
            //                + "\n\nFrom: " + model.Start.ToShortDateString() + " " + model.Start.ToShortTimeString()
            //                + "\n\nTo: " + model.End.ToShortDateString() + " " + model.End.ToShortTimeString();
            //await _emailSender.SendEmailAsync(email, subject, message);
            return vacation;
          
        }

        public async Task CancelVacationAsync(int vacationId)
        {
            var getVacation = await _db.UserVacations.Include(u => u.User).FirstOrDefaultAsync(u => u.Id == vacationId);
          
            int hours = CountHours(getVacation.Start, getVacation.End);
            var isApproved = getVacation.Color;
            if (isApproved == "Green") //approved
            {               
                getVacation.User.RemainingDayOff = (int.Parse(getVacation.User.RemainingDayOff) + hours).ToString();
            }       
            _db.UserVacations.Remove(getVacation);            
            await _db.SaveChangesAsync();

            //var email = GetEmailAdmin();
            //var subject = "[Vacation Tracking] - " + getVacation.User.FirstName + " " + getVacation.User.LastName + " canceled a vacation!";
            //var message = "\n\nVacation Detail: "
            //                + "\n\nTitle: " + getVacation.Title
            //                + "\n\nFrom: " + getVacation.Start.ToShortDateString() + " " + getVacation.Start.ToShortTimeString()
            //                + "\n\nTo: " + getVacation.End.ToShortDateString() + " " + getVacation.End.ToShortTimeString();
            //await _emailSender.SendEmailAsync(email, subject, message);
        }

        public async Task<UserVacation> UpdateVacationAsync(UserVacationViewModel model)
        {
            var getVacation = await _db.UserVacations.Include(u => u.User).FirstOrDefaultAsync(u => u.Id == model.Id);

            var isApproved = getVacation.Color;
            int hours = CountHours(getVacation.Start, getVacation.End);
            if(isApproved=="Green") //approved
            {
                getVacation.Title = model.Title;
                getVacation.Start = model.Start;
                getVacation.End = model.End;
                getVacation.Color = "blue"; //Return pending status
                getVacation.User.RemainingDayOff = (int.Parse(getVacation.User.RemainingDayOff) + hours).ToString();
                await _db.SaveChangesAsync();
            }
            else if(isApproved=="blue")
            {
                getVacation.Title = model.Title;
                getVacation.Start = model.Start;
                getVacation.End = model.End;
                getVacation.Color = "blue"; //Return pending status
                await _db.SaveChangesAsync();
            }

            //var email = GetEmailAdmin();
            //var subject = "[Vacation Tracking] - " + getVacation.User.FirstName + " " + getVacation.User.LastName + "has updated a vacation!";
            //var message = "\n\nVacation Request Detail: "
            //                + "\n\nReason: " + model.Title
            //                + "\n\nFrom: " + model.Start.ToShortDateString() + " " + model.Start.ToShortTimeString()
            //                + "\n\nTo: " + model.End.ToShortDateString() + " " + model.End.ToShortTimeString();
            //await _emailSender.SendEmailAsync(email, subject, message);
            return getVacation;
        }


        #region admin

        public async Task<List<UserVacation>> GetAllVacationAsync()
        {
            return await _db.UserVacations.Include(u => u.User).ToListAsync();
        }


        public async Task<UserVacation> GetVacationByVacationIdAsync(int vacationId)
        {
            var getVacation = await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == vacationId);
            return getVacation;
        }
        public async Task<List<UserVacation>> GetRequestVacationAsync()
        {
            return await _db.UserVacations.Include(u => u.User).Where(u => u.Color == "blue").ToListAsync();
        }


        public async Task<List<UserVacation>> GetVacationByUserIdAsync(string userId)
        {

            return await _db.UserVacations.Where(u => u.UserId == userId).ToListAsync();
        }
        public async Task ApproveVacation(int vacationId)
        {
            var getVacation = await _db.UserVacations.Include(u => u.User).Include(x => x.User.User).FirstOrDefaultAsync(u => u.Id == vacationId);
            int hours = CountHours(getVacation.Start, getVacation.End);
            int hoursleft = int.Parse(getVacation.User.RemainingDayOff) - hours;


            //var email = getVacation.User.User.Email;
            //var subject = "[Vacation Tracking] - Your vacation is approved!";
            //var message = "\nHi " + getVacation.User.FirstName
            //                + ",\n\n Your vacation has been approved,"
            //                + "\n\nVacation Request Detail: "
            //                + "\n\nTitle: " + getVacation.Title
            //                + "\n\nFrom: " + getVacation.Start.ToShortDateString() + " " + getVacation.Start.ToShortTimeString()
            //                + "\n\nTo: " + getVacation.End.ToShortDateString() + " " + getVacation.End.ToShortTimeString()
            //                + "\n\nYour Remaining Time: " + hoursleft.ToString() + " hour(s)";
            //await _emailSender.SendEmailAsync(email, subject, message);

            getVacation.Color = "Green";
            getVacation.User.RemainingDayOff = hoursleft.ToString();
            await _db.SaveChangesAsync();
        }
        public async Task DisapproveVacation(int vacationId, string reason)
        {
            var vacation = await _db.UserVacations.Include(u => u.User).Include(x => x.User.User).FirstOrDefaultAsync(u => u.Id == vacationId);
            _db.UserVacations.Remove(vacation);

            int hours = CountHours(vacation.Start, vacation.End);

            //var email = vacation.User.User.Email;
            //var subject = "[Vacation Tracking] - Your vacation is disapproved!";
            //var message = "\nHi " + vacation.User.FirstName
            //                + ",\n\n Your vacation has been disapproved,"
            //                 + "\n\nVacation Request Detail: "
            //                + "\n\nTitle: " + vacation.Title
            //                + "\n\nFrom: " + vacation.Start.ToShortDateString() + " " + vacation.Start.ToShortTimeString()
            //                + "\n\nTo: " + vacation.End.ToShortDateString() + " " + vacation.End.ToShortTimeString()
            //                + "\n\n Reason: " + reason ;
            //await _emailSender.SendEmailAsync(email, subject, message);

            await _db.SaveChangesAsync();
        }

        public int CountHours(DateTime start, DateTime end)
        {
            var hours = 0;
            for (var i = start; i < end; i = i.AddHours(1))
            {
                if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours < 12 || i.TimeOfDay.Hours >= 13 && i.TimeOfDay.Hours < 18)
                    {
                        hours++;
                    }
                }
            }
            return hours;
        }
        public  string GetEmailAdmin()
        {
            string allEmail = "";
            var getEmail =    _db.UserProfiles.Include(u => u.User).Where(u => u.Position == "ADMIN").Select(u => u.User.Email).ToArray();
            foreach(string email in getEmail)
            {
                allEmail = allEmail + "," + email;
            }
            return allEmail;
        }

        
        #endregion
    }
}
