﻿using FuturifyVacation.ServicesInterfaces;
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

        public async Task AddVacationAsync(UserVacationViewModel model, string userId)
        {

            var vacation = new UserVacation
            {
                Id = model.Id,
                Title = model.Title,
                UserId = userId,
                Start = model.Start,
                End = model.End,
                Color = model.Color
            };
            await _db.UserVacations.AddAsync(vacation);
            await _db.SaveChangesAsync();
        }

        public Task<UserVacation> DeleteVacationAsync(int vacationId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserVacation> UpdateVacationAsync(UserVacationViewModel model)
        {
            var getVacation = await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == model.Id);
            getVacation.Title = model.Title;
            getVacation.Start = model.Start;
            getVacation.End = model.End;
            getVacation.Color = "blue"; //Return pending status
            await _db.SaveChangesAsync();
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
            //var subject = "[Vacation Tracking] - Your vacation has been approved!";
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
            //var subject = "[Vacation Tracking] - Your vacation has been disapproved!";
            //var message = "\nHi " + vacation.User.FirstName
            //                + ",\n\n Your vacation has been disapproved,"
            //                 + "\n\nVacation Request Detail: "
            //                + "\n\nTitle: " + vacation.Title
            //                + "\n\nFrom: " + vacation.Start.ToShortDateString() + " " + vacation.Start.ToShortTimeString()
            //                + "\n\nTo: " + vacation.End.ToShortDateString() + " " + vacation.End.ToShortTimeString()
            //                + "\n\nReason: " + reason;
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
        #endregion
    }
}
