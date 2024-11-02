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

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Obtener los claims del usuario autenticado
            var userClaims = User.Claims;

            // Obtener el email del usuario
            var email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Log para verificar el email obtenido
            Console.WriteLine($"Email del usuario: {email}");

            // Obtener el token de acceso del usuario autenticado
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            // Verificar si el token de acceso es válido
            Console.WriteLine($"Access Token: {accessToken}");

            // Obtener los datos de género y fecha de cumpleaños desde PeopleService
            DateOnly? fechaNac = null;
            string genero = null;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var (birthday, gender) = await peopleService.GetUserProfile(accessToken);
                fechaNac = birthday;
                genero = gender;

                // Log para verificar los valores obtenidos
                Console.WriteLine($"Fecha de nacimiento en el controlador: {fechaNac}");
                Console.WriteLine($"Género en el controlador: {genero}");
            }

            // Guardar el perfil del usuario en la base de datos (usando UserService)
            var nombre = userClaims.FirstOrDefault(c => c.Type == "name")?.Value;

            Console.WriteLine($"Nombre del usuario: {nombre}");

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(nombre))
            {
                await userService.SaveUserProfileAsync(email, nombre, genero, fechaNac);
                Console.WriteLine("Perfil guardado exitosamente en la base de datos.");
            }
            else
            {
                Console.WriteLine("Error: El email o el nombre son nulos.");
            }

            return View();
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
    

