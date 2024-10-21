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

        public AccountController(UserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Obtener los claims del usuario autenticado
            var userClaims = User.Claims;

            var email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var nombre = userClaims.FirstOrDefault(c => c.Type == "name")?.Value;
            var genero = userClaims.FirstOrDefault(c => c.Type == "gender")?.Value;
            var fechaNacimiento = userClaims.FirstOrDefault(c => c.Type == "birthdate")?.Value;

            // Convertir la fecha de nacimiento a DateOnly
            DateOnly? fechaNac = null;
            if (DateOnly.TryParse(fechaNacimiento, out var parsedDate))
            {
                fechaNac = parsedDate;
            }

            // Guardar el perfil del usuario en la base de datos (usando UserService)
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(nombre))
            {
                await userService.SaveUserProfileAsync(email, nombre, genero, fechaNac);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("Profile", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = Url.Action("Predict", "Prediction") },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }

}
    

