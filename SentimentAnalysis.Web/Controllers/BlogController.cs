using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.Data.Entities;
using SentimentAnalysis.Service;
using SentimentAnalysis.Web.Models;


namespace SentimentAnalysis.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;
        private readonly UserService _userService;

        public BlogController(BlogService blogService,UserService userService) {
            _blogService = blogService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            var posts=_blogService.GetPosts();
            return View(posts);
        }
        public async Task<IActionResult> CrearPost()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearPost(PostModelView postModel)
        {
        
            User? user = await _userService.GetUserByUsername(postModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View(postModel);
            }
            var post = new Post
            {
                Text = postModel.Text,
                Title = postModel.Title,
                UserId = user.Id,
                PostDate = await _blogService.SetDatePost()
            };
         

            if (ModelState.IsValid)
            {
                await _blogService.CreatePost(post);
                return RedirectToAction("Index");
            }
            return View(post);
        }
    }
}
