using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2.Flows;
using System.Threading;
using FuturifyVacation.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using FuturifyVacation.Models;
using Microsoft.AspNetCore.Authorization;
using FuturifyVacation.ServicesInterfaces;
using System.Security.Claims;
using Google.Apis.Auth.OAuth2.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuturifyVacation.Controllers
{
    [Authorize]
    [Route("api/googlecalendars")]
    public class GoogleCalendarsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IGoogleService _googleService;
        public GoogleCalendarsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IGoogleService googleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _googleService = googleService;
        }

        [HttpPost("addevent")]
        public async Task<UserVacationViewModel> AddEvent([FromBody]UserVacationViewModel model)
        {
            string clientId = "898437920378-seajv3gkh33evbq34kst2r91sk4qag0m.apps.googleusercontent.com";
            string clientSecret = "BiGyH2JIj2iCIldk_wXaIrWk";
           
            var getUser = await _userManager.GetUserAsync(User);
            var info = await _signInManager.GetExternalLoginInfoAsync();
            //var gettoken = info.AuthenticationTokens.FirstOrDefault(u => u.Name == "access_token");
            //var accessToken = gettoken.Value;
            //var getExpireTime = info.AuthenticationTokens.FirstOrDefault(u => u.Name == "expires_at");
            //DateTime expireTime = Convert.ToDateTime(getExpireTime);
            
            if (getUser != null)
            {                             
                var getToken = await _googleService.GetTokenAsync(getUser.Id);
               var credential =  GetCredential(getToken.AccessToken, getToken.IssuedAt, clientId, clientSecret);               
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Futurify Vacation",
                });

                // Define parameters of request.

                var newEvent = new Event
                {                    
                    Summary = "Day Off",
                    Description = model.Title,
                    Start = new EventDateTime
                    {
                        DateTime = model.Start
                    },
                    End = new EventDateTime
                    {
                        DateTime = model.End
                    },
                   ColorId="9",
                };
                EventsResource.InsertRequest requestInsertEvent = service.Events.Insert(newEvent, "primary");              
                requestInsertEvent.Execute();
                //if(requestInsertEvent!=null)
                //{
                //   await _googleService.SaveCalendarId(model.Id, newEvent.Id );
                //}
                
            }
            return model;
        }

 
        private  UserCredential GetCredential(string aToken, DateTime issuedAt, string clientId, string clientSecret)
        {
            //var token = new Google.Apis.Auth.OAuth2.Responses.TokenResponse()
            //{
            //    AccessToken = aToken,
            //    ExpiresInSeconds = 3600,
            //    IssuedUtc = issuedAt.ToUniversalTime()
            //};
            var token = new TokenResponse
            {
                AccessToken = aToken,
                RefreshToken = aToken
            };
            var fakeflow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                }
            });

            UserCredential credential = new UserCredential(fakeflow, Environment.UserName, token);

           
            //if (issuedAt.AddSeconds(3540) < DateTime.Now)
            //{
            //    await credential.RefreshTokenAsync(CancellationToken.None);
            //    await credential.GetAccessTokenForRequestAsync();
            //    // TODO update issuedat
            //    // TODO update access token
            //}

            return credential;
        }
    }
}
