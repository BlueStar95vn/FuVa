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
        public async Task<int> CheckVacationInMonth(UserVacationViewModel model, string userId)
        {
            int isLarger = 0;
            int bookingTime = CountHours(model.Start, model.End);

            int totalHours = 0;
            var vacationInMonth = await _db.UserVacations.Where(u => u.UserId == userId
                                       && ((u.Start.Month == model.Start.Month && u.Start.Year == model.Start.Year)
                                       || u.End.Month == model.End.Month && u.End.Year == model.End.Year)
                                      ).ToListAsync();
            foreach (var vacation in vacationInMonth)
            {
                if ((vacation.Start.Month == vacation.End.Month))
                {
                    totalHours = totalHours + vacation.Hours;
                }
                if (vacation.Start.Month < model.Start.Month)
                {
                    DateTime startDate = new DateTime(vacation.End.Year, vacation.End.Month, 1, 0, 0, 1);
                    int hours = CountHours(startDate, vacation.End);
                    totalHours = totalHours + hours;
                }
                if (vacation.End.Month > model.Start.Month)
                {
                    DateTime endDate = new DateTime(vacation.Start.Year, vacation.Start.Month, DateTime.DaysInMonth(vacation.Start.Year, vacation.Start.Month), 23, 59, 59);
                    int hours = CountHours(vacation.Start, endDate);
                    totalHours = totalHours + hours;
                }
            }
            var getSetting = await _db.Settings.FirstOrDefaultAsync(u => u.Id == 1);

            if (vacationInMonth.Count() >= getSetting.DurationInMonth || totalHours >= getSetting.DurationInMonth * getSetting.HoursADay || (totalHours + bookingTime) > getSetting.DurationInMonth * getSetting.HoursADay)
            {
                isLarger = 1;
            }

            return isLarger;
        }


        

        public async Task<UserVacation> AddVacationAsync(UserVacationViewModel model, string userId)
        {
            int hours = CountHours(model.Start, model.End);
            var vacation = new UserVacation
            {
                Title = model.Title,
                UserId = userId,
                Start = model.Start,
                End = model.End,
                Color = model.Color,
                Hours = hours
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
            var userVacation = await _db.UserVacations.Include(u => u.User).FirstOrDefaultAsync(u => u.Id == vacationId);

            int hours = CountHours(userVacation.Start, userVacation.End);
            var isApproved = userVacation.Color;
            if (isApproved == "Green") //approved
            {
                userVacation.User.RemainingDayOff = (int.Parse(userVacation.User.RemainingDayOff) + hours).ToString();
            }
            _db.UserVacations.Remove(userVacation);
            await _db.SaveChangesAsync();

            //var email = GetEmailAdmin();
            //var subject = "[Vacation Tracking] - " + userVacation.User.FirstName + " " + userVacation.User.LastName + " canceled a vacation!";
            //var message = "\n\nVacation Detail: "
            //                + "\n\nTitle: " + userVacation.Title
            //                + "\n\nFrom: " + userVacation.Start.ToShortDateString() + " " + userVacation.Start.ToShortTimeString()
            //                + "\n\nTo: " + userVacation.End.ToShortDateString() + " " + userVacation.End.ToShortTimeString();
            //await _emailSender.SendEmailAsync(email, subject, message);
        }

        public async Task<UserVacation> UpdateVacationAsync(UserVacationViewModel model)
        {
            var userVacation = await _db.UserVacations.Include(u => u.User).FirstOrDefaultAsync(u => u.Id == model.Id);

            var isApproved = userVacation.Color;
            int hours = CountHours(userVacation.Start, userVacation.End);
            if (isApproved == "Green") //approved
            {
                userVacation.Title = model.Title;
                userVacation.Start = model.Start;
                userVacation.End = model.End;
                userVacation.Color = "blue"; //Return pending status
                userVacation.User.RemainingDayOff = (int.Parse(userVacation.User.RemainingDayOff) + hours).ToString();
                userVacation.Hours = hours;
                await _db.SaveChangesAsync();
            }
            else if (isApproved == "blue")
            {
                userVacation.Title = model.Title;
                userVacation.Start = model.Start;
                userVacation.End = model.End;
                userVacation.Color = "blue"; //Return pending status
                userVacation.Hours = hours;
                await _db.SaveChangesAsync();
            }

            //var email = GetEmailAdmin();
            //var subject = "[Vacation Tracking] - " + userVacation.User.FirstName + " " + userVacation.User.LastName + "has updated a vacation!";
            //var message = "\n\nVacation Request Detail: "
            //                + "\n\nReason: " + model.Title
            //                + "\n\nFrom: " + model.Start.ToShortDateString() + " " + model.Start.ToShortTimeString()
            //                + "\n\nTo: " + model.End.ToShortDateString() + " " + model.End.ToShortTimeString();
            //await _emailSender.SendEmailAsync(email, subject, message);
            return userVacation;
        }


        #region admin

        public async Task<List<UserVacation>> GetAllVacationAsync()
        {
            return await _db.UserVacations.Include(u => u.User).ToListAsync();
        }


        public async Task<UserVacation> GetVacationByVacationIdAsync(int vacationId)
        {
            var userVacation = await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == vacationId);
            return userVacation;
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
            var userVacation = await _db.UserVacations.Include(u => u.User).Include(x => x.User.User).FirstOrDefaultAsync(u => u.Id == vacationId);
            int hours = CountHours(userVacation.Start, userVacation.End);
            int hoursleft = int.Parse(userVacation.User.RemainingDayOff) - hours;


            //var email = userVacation.User.User.Email;
            //var subject = "[Vacation Tracking] - Your vacation is approved!";
            //var message = "\nHi " + userVacation.User.FirstName
            //                + ",\n\n Your vacation has been approved,"
            //                + "\n\nVacation Request Detail: "
            //                + "\n\nTitle: " + userVacation.Title
            //                + "\n\nFrom: " + userVacation.Start.ToShortDateString() + " " + userVacation.Start.ToShortTimeString()
            //                + "\n\nTo: " + userVacation.End.ToShortDateString() + " " + userVacation.End.ToShortTimeString()
            //                + "\n\nYour Remaining Time: " + hoursleft.ToString() + " hour(s)";
            //await _emailSender.SendEmailAsync(email, subject, message);

            userVacation.Color = "Green";
            userVacation.User.RemainingDayOff = hoursleft.ToString();
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

        public async Task<int> ApprovedVacationInMonth(int vacationId, string userId)
        {
            var userVacation = await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == vacationId);
            var vacationInMonth = await _db.UserVacations.Where(u => u.UserId == userId
                                     && ((u.Start.Month == userVacation.Start.Month && u.Start.Year == userVacation.Start.Year)
                                     || u.End.Month == userVacation.End.Month && u.End.Year == userVacation.End.Year)
                                     && u.Color == "Green").ToListAsync();
            int totalHours = 0;
            foreach (var vacation in vacationInMonth)
            {
                if ((vacation.Start.Month == vacation.End.Month))
                {
                    totalHours = totalHours + vacation.Hours;
                }
                if (vacation.Start.Month < userVacation.Start.Month)
                {
                    DateTime startDate = new DateTime(vacation.End.Year, vacation.End.Month, 1, 0, 0, 1);
                    int hours = CountHours(startDate, vacation.End);
                    totalHours = totalHours + hours;
                }
                if (vacation.End.Month > userVacation.Start.Month)
                {
                    DateTime endDate = new DateTime(vacation.Start.Year, vacation.Start.Month, DateTime.DaysInMonth(vacation.Start.Year, vacation.Start.Month), 23, 59, 59);
                    int hours = CountHours(vacation.Start, endDate);
                    totalHours = totalHours + hours;
                }

            }
            return totalHours;

        }

        public async Task<List<TeamDetail>> CheckTeamOnDate(int vacationId, string userId)
        {
            var teamDetail = await _db.TeamDetails.Where(u => u.UserId == userId).ToListAsync();
            var teamIds = teamDetail.Select(u => u.TeamId).ToList();

            var userVacation = await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == vacationId);
            //var getAllMemberOnDate = await _db.UserVacations.Where(u => u.UserId != userId && u.Start.Month == userVacation.Start.Month && u.Start.Year == userVacation.Start.Year && u.Color == "Green").ToListAsync();
            var vacationOnDate = _db.UserVacations.Where(u => u.User.User.TeamDetail.Any(t => teamIds.Contains(t.TeamId)) && u.Start < userVacation.End && userVacation.Start < u.End && u.UserId != userId);

            var teamMember = await vacationOnDate.Select(u => u.UserId).ToListAsync();

            var teamMemberListInfo = await _db.TeamDetails.Include(u => u.TeamMember).Include(u => u.TeamMember.UserProfile).Include(u => u.Team).Where(u => teamMember.Contains(u.UserId) && teamIds.Contains(u.TeamId)).ToListAsync();

            return teamMemberListInfo;
        }

        public int CountHours(DateTime start, DateTime end)
        {
            var setting = _db.Settings.FirstOrDefault(u => u.Id == 1);

            var hours = 0;
            for (var i = start; i < end; i = i.AddHours(1))
            {
                if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (i.TimeOfDay.Hours >= setting.StartAm && i.TimeOfDay.Hours < setting.EndAm || i.TimeOfDay.Hours >= setting.StartPm && i.TimeOfDay.Hours < setting.EndPm)
                    {
                        hours++;
                    }
                }
            }
            return hours;
        }
        public string GetEmailAdmin()
        {
            string allEmail = "";
            var adminEmail = _db.UserProfiles.Include(u => u.User).Where(u => u.Position == "ADMIN").Select(u => u.User.Email).ToArray();
            foreach (string email in adminEmail)
            {
                allEmail = allEmail + "," + email;
            }
            return allEmail;
        }







        #endregion
    }
}
