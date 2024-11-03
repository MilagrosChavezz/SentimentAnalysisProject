using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.Data.Entities;
using SentimentAnalysis.Service;
using SentimentAnalysis.Web.Models;
using System.Security.Claims;
using SentimentAnalysis.Entitys;
using Microsoft.AspNetCore.Authentication;


namespace SentimentAnalysis.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;
        private readonly UserService _userService;
        private readonly PeopleService _peopleService;
        private readonly MLAnalysis lAnalysis;

        public BlogController(BlogService blogService, UserService userService, MLAnalysis lAnalysis, PeopleService peopleService)
        {
            _blogService = blogService;
            _userService = userService;
            _peopleService = peopleService;
            this.lAnalysis = lAnalysis;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _blogService.GetPosts();
            var userClaims = User.Claims;

            var email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            Console.WriteLine($"Email del usuario: {email}");

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            Console.WriteLine($"Access Token: {accessToken}");

            DateOnly? fechaNac = null;
            string genero = null;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var (birthday, gender) = await _peopleService.GetUserProfile(accessToken);
                fechaNac = birthday;
                genero = gender;

            }

            var nombre = userClaims.FirstOrDefault(c => c.Type == "name")?.Value;

            Console.WriteLine($"Nombre del usuario: {nombre}");

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(nombre))
            {
                await _userService.SaveUserProfileAsync(email, nombre, genero, fechaNac);
                Console.WriteLine("Perfil guardado exitosamente en la base de datos.");
            }
            else
            {
                Console.WriteLine("Error: El email o el nombre son nulos.");
            }
            return View(posts);
        }
        public async Task<IActionResult> CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostModelView postModel)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userService.GetUserByEmailAsync(userEmail);

            var data = new SentimentAnalysis.Entitys.Data { clean_text = postModel.Text };
           
            bool is_depression = lAnalysis.Predict(data).is_depression;
            if (user == null)
            {
                
                ModelState.AddModelError(string.Empty, "User not found.Please login");
                return View(postModel);
            }
            var post = new Post
            {
                Text = postModel.Text,
                Title = postModel.Title,
                UserId = user.Id,
                IsDepression = is_depression,
                PostDate = await _blogService.SetDatePost()
            };

            await _blogService.CreatePost(post);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string keyword)
        {
            var results = await _blogService.SearchPosts(keyword);
            return View("Index", results);
        }

    }
}
