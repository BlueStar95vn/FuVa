using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FuturifyVacation.Models.ViewModels;
using FuturifyVacation.ServicesInterfaces;
using FuturifyVacation.Models;
using Microsoft.AspNetCore.Identity;
using FuturifyVacation.Models.BindingModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/emailsenders")]
    public class EmailSendersController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVacationService _vacationService;
        public EmailSendersController(IEmailSender emailSender, IEmployeeService employeeService, UserManager<ApplicationUser> userManager, IVacationService vacationService)
        {
            _emailSender = emailSender;
            _employeeService = employeeService;
            _userManager = userManager;
            _vacationService = vacationService;
        }

        [HttpPost("bookvacation/{vacationId}")]
        public async Task BookVacation([FromBody]UserVacationViewModel model, string vacationId)
        {
            var email = _emailSender.GetEmailAdmin();
            var getUser =  _userManager.GetUserId(User);
            var getEmployee = await _employeeService.GetByIdAsync(getUser);
            var subject = "[Vacation Tracking] - " + getEmployee.FirstName + " " + getEmployee.LastName + " booked a vacation!";
            var message = "\n\nVacation Request Detail: "
                            + "\n\nReason: " + model.Title
                            + "\n\nFrom: " + model.Start.ToShortDateString() + " " + model.Start.ToShortTimeString()
                            + "\n\nTo: " + model.End.ToShortDateString() + " " + model.End.ToShortTimeString();
            await _emailSender.SendEmailAsync(email, subject, message);
        }
       
        [HttpPost("updatevacation")]
        public async Task UpdateVacation([FromBody]UserVacationViewModel model)
        {
            var getUser = _userManager.GetUserId(User);
            var getEmployee = await _employeeService.GetByIdAsync(getUser);

            var email = _emailSender.GetEmailAdmin();
            var subject = "[Vacation Tracking] - " + getEmployee.FirstName + " " + getEmployee.LastName + " has updated a vacation!";
            var message = "\n\nVacation Request Detail: "
                            + "\n\nReason: " + model.Title
                            + "\n\nFrom: " + model.Start.ToShortDateString() + " " + model.Start.ToShortTimeString()
                            + "\n\nTo: " + model.End.ToShortDateString() + " " + model.End.ToShortTimeString();
            await _emailSender.SendEmailAsync(email, subject, message);
        }


        [HttpPost("cancelvacation")]
        public async Task CancelVacation([FromBody]UserVacationViewModel model)
        {
            var getUser = _userManager.GetUserId(User);
            var getEmployee = await _employeeService.GetByIdAsync(getUser);

            var email = _emailSender.GetEmailAdmin();           
            var subject = "[Vacation Tracking] - " + getEmployee.FirstName + " " + getEmployee.LastName + " canceled a vacation!";
            var message = "\n\nVacation Detail: "
                            + "\n\nTitle: " + model.Title
                            + "\n\nFrom: " + model.Start.ToShortDateString() + " " + model.Start.ToShortTimeString()
                            + "\n\nTo: " + model.End.ToShortDateString() + " " + model.End.ToShortTimeString();
            await _emailSender.SendEmailAsync(email, subject, message);
        }


        
        /*----------------ADMIN---------------*/

        [Authorize(Roles = "ADMIN")]
        [HttpPost("addemployee")]
        public async Task AddEmployee([FromBody]EmployeeViewModel model)
        {
            var subject = "[Vacation Tracking] Your account is created";
            var message = "Hi " + model.FirstName + " " + model.LastName + "," +
                "\nLogin with your email! http://localhost:63237/#/login";
            await _emailSender.SendEmailAsync(model.Email, subject, message);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("approvevacation")]
        public async Task ApproveVaction([FromBody]UserVacationViewModel model)
        {
            var getEmployee = await _employeeService.GetByIdAsync(model.UserId);
            var email = getEmployee.User.Email;
            var subject = "[Vacation Tracking] - Your vacation is approved!";
            var message = "\nHi " + model.FirstName
                            + ",\n\n Your vacation has been approved,"
                            + "\n\nYour Vacation Detail: "
                            + "\n\nTitle: " + model.Title
                            + "\n\nFrom: " + model.Start.ToShortDateString() + " " + model.Start.ToShortTimeString()
                            + "\n\nTo: " + model.End.ToShortDateString() + " " + model.End.ToShortTimeString();
            await _emailSender.SendEmailAsync(email, subject, message);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("disapprovevacation/{id}")]
        public async Task DisapproveVaction(int id, [FromBody]DisapproveBindingModel model)
        {
            var getVacation = await _vacationService.GetVacationByVacationIdAsync(id);
            var getEmployee = await _employeeService.GetByIdAsync(getVacation.UserId);
            var email = getEmployee.User.Email;
            var subject = "[Vacation Tracking] - Your vacation is disapproved!";
            var message = "\nHi " + getEmployee.FirstName
                            + ",\n\n Your vacation has been disapproved,"
                             + "\n\nVacation Request Detail: "
                            + "\n\nTitle: " + getVacation.Title
                            + "\n\nFrom: " + getVacation.Start.ToShortDateString() + " " + getVacation.Start.ToShortTimeString()
                            + "\n\nTo: " + getVacation.End.ToShortDateString() + " " + getVacation.End.ToShortTimeString()
                            + "\n\n Reason: " + model.Reason;
            await _emailSender.SendEmailAsync(email, subject, message);
        }
    }
}
