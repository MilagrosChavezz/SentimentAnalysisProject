using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.Service;
using System.Security.Claims;

namespace SentimentAnalysis.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService userService;

        public AccountController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("Predict", "Prediction");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = Url.Action("Predict", "Prediction") },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserIfNotExists()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var fullName = User.FindFirst(ClaimTypes.Name)?.Value;
            var birthdayClaim = User.FindFirst("birthdate")?.Value;
            var genderClaim = User.FindFirst("gender")?.Value;
            var countryClaim = User.FindFirst("locale")?.Value;

            DateTime? birthday = null;
            if (DateTime.TryParse(birthdayClaim, out var parsedBirthday))
            {
                birthday = parsedBirthday;
            }

            if (email != null && fullName != null && genderClaim != null && countryClaim != null)
            {
                await userService.CreateUserIfNotExists(email, fullName, birthday, genderClaim, countryClaim);
                return Ok();
            }

            return BadRequest("Faltan datos necesarios.");
        }
    }

}
    

