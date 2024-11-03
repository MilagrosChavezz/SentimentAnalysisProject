using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.Service;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SentimentAnalysis.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService userService;
        private readonly PeopleService peopleService;

        public AccountController(UserService userService, PeopleService peopleService)
        {
            this.userService = userService;
            this.peopleService = peopleService;
        }

        [HttpPost]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("Index", "Blog");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = Url.Action("Index", "Blog") },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }

}
    

