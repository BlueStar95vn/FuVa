using FuturifyVacation.Data;
using FuturifyVacation.Models;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuturifyVacation.Models.ViewModels;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using System.Threading;

namespace FuturifyVacation.Services
{
    public class GoogleService : IGoogleService
    {
        private ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private string clientId = "898437920378-seajv3gkh33evbq34kst2r91sk4qag0m.apps.googleusercontent.com";
        private string clientSecret = "BiGyH2JIj2iCIldk_wXaIrWk";
        private string calendarId = "8e1jo2hsh54cd588k9tlk2ht64@group.calendar.google.com";
        public GoogleService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        public async Task<UserGoogleToken> GetTokenAsync(string userId)
        {
            return await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task SaveEventId(string googleCalendarId, int id)
        {
            var getVacation = await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == id);
            getVacation.GoogleCalendarId = googleCalendarId;
            await _db.SaveChangesAsync();
        }

        public async Task SaveToken(string userId, string token, DateTime issuedAt)
        {
            var getToken = await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.UserId == userId);
            if (getToken == null)
            {
                var newToken = new UserGoogleToken
                {
                    UserId = userId,
                    AccessToken = token,
                    IssuedAt = issuedAt.ToUniversalTime()
                };
                await _db.UserGoogleToken.AddAsync(newToken);
            }
            else
            {
                getToken.AccessToken = token;
                getToken.IssuedAt = issuedAt.ToUniversalTime();
            }
            await _db.SaveChangesAsync();
        }
        public async Task AddEvent(UserVacationViewModel model, string userId)
        {
            var getToken = await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.UserId == userId);
            if (getToken != null)
            {
                UserCredential credential = await GetCredential(getToken.AccessToken, getToken.IssuedAt, clientId, clientSecret);
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Futurify Vacation",
                });

                // Define parameters of request.

                var newEvent = new Event
                {
                    Summary = model.FirstName + " " + model.LastName,
                    Description = model.Title,
                    Start = new EventDateTime
                    {
                        DateTime = model.Start
                    },
                    End = new EventDateTime
                    {
                        DateTime = model.End
                    },
                    ColorId = "10",
                };
                EventsResource.InsertRequest requestInsertEvent = service.Events.Insert(newEvent, calendarId);
                var insert = await requestInsertEvent.ExecuteAsync();
                model.GoogleCalendarId = insert.Id;
            }
        }

        public async Task DeleteEvent(string GoogleEventId, string userId)
        {

            var getToken = await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.UserId == userId);
            if (getToken != null && GoogleEventId != null)
            {
                UserCredential credential = await GetCredential(getToken.AccessToken, getToken.IssuedAt, clientId, clientSecret);
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Futurify Vacation",
                });
                EventsResource.DeleteRequest deleteEvent = service.Events.Delete(calendarId, GoogleEventId);
                await deleteEvent.ExecuteAsync();
            }
        }


        public async Task UpdateEvent(UserVacationViewModel model, string googleCalendarId, string userId)
        {
            var getEvent = await _db.UserVacations.FirstOrDefaultAsync(u => u.GoogleCalendarId == googleCalendarId);
            var getToken = await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.UserId == userId);
            if (getEvent != null && getToken != null)
            {
                UserCredential credential = await GetCredential(getToken.AccessToken, getToken.IssuedAt, clientId, clientSecret);
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Futurify Vacation",
                });

                // Define parameters of request.

                var newEvent = new Event
                {
                    Summary = "Day Off: Pending",
                    Description = model.Title,
                    Start = new EventDateTime
                    {
                        DateTime = model.Start
                    },
                    End = new EventDateTime
                    {
                        DateTime = model.End
                    },
                    ColorId = "9",
                };
                EventsResource.UpdateRequest updateEvent = service.Events.Update(newEvent, calendarId, getEvent.GoogleCalendarId);
                var update = await updateEvent.ExecuteAsync();
            }
        }
        public async Task ApproveVacation(UserVacationViewModel model, int vacationId)
        {
            var getEvent = await _db.UserVacations.FirstOrDefaultAsync(u => u.Id == vacationId);
            var getToken = await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.UserId == getEvent.UserId);
            if (getEvent != null && getToken != null)
            {
                UserCredential credential = await GetCredential(getToken.AccessToken, getToken.IssuedAt, clientId, clientSecret);
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Futurify Vacation",
                });

                // Define parameters of request.

                var newEvent = new Event
                {
                    Summary = "Day Off: Approved",
                    Description = model.Title,
                    Start = new EventDateTime
                    {
                        DateTime = model.Start
                    },
                    End = new EventDateTime
                    {
                        DateTime = model.End
                    },
                    ColorId = "10",
                };
                EventsResource.UpdateRequest approveEvent = service.Events.Update(newEvent, calendarId, getEvent.GoogleCalendarId); //"primary"
                approveEvent.SendNotifications = true;
                var approve = await approveEvent.ExecuteAsync();
            }
        }







        private async Task<UserCredential> GetCredential(string aToken, DateTime issuedAt, string clientId, string clientSecret)
        {
            var token = new TokenResponse()
            {
                AccessToken = aToken,
                ExpiresInSeconds = 3600,
                IssuedUtc = issuedAt,
                RefreshToken = aToken
            };
            //var token = new TokenResponse
            //{
            //    AccessToken = aToken,
            //    RefreshToken = aToken
            //};
            var fakeflow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                }
            });


            UserCredential credential = new UserCredential(fakeflow, Environment.UserName, token);

            if (credential.Token.IsExpired(credential.Flow.Clock))
            {
                //await credential.RefreshTokenAsync(CancellationToken.None);

            }
            if (issuedAt.AddSeconds(3540) < DateTime.Now)
            {


                var refresh = await credential.RefreshTokenAsync(CancellationToken.None);
                await credential.GetAccessTokenForRequestAsync();

                //var getToken = await _db.UserGoogleToken.FirstOrDefaultAsync(u => u.AccessToken == aToken);
                //getToken.IssuedAt = DateTime.Now.ToUniversalTime();
                //await _db.SaveChangesAsync();



                // TODO update issuedat
                // TODO update access token
            }

            return credential;
        }


    }
}
