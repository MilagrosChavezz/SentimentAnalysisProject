using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.Data.Entities;
using SentimentAnalysis.Service;
using SentimentAnalysis.Web.Models;
using System.Security.Claims;
using SentimentAnalysis.Entitys;


namespace SentimentAnalysis.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;
        private readonly UserService _userService;
        private readonly MLAnalysis lAnalysis;

        public BlogController(BlogService blogService, UserService userService, MLAnalysis lAnalysis)
        {
            _blogService = blogService;
            _userService = userService;
            this.lAnalysis = lAnalysis;
        }

        public IActionResult Index()
        {
            var posts = _blogService.GetPosts();
            return View(posts);
        }

        public async Task<IActionResult> CrearPost()
        {
            return View();
        }

        // ... other code ...

        [HttpPost]
        public async Task<IActionResult> CrearPost(PostModelView postModel)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userService.GetUserByEmailAsync(userEmail);

            // Create a Data object from postModel.Text
            var data = new SentimentAnalysis.Entitys.Data { clean_text = postModel.Text };
           
            bool is_depression = lAnalysis.Predict(data).is_depression;

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
    }
}
