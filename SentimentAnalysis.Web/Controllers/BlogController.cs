using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.Data.Entities;
using SentimentAnalysis.Service;


namespace SentimentAnalysis.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;

        public BlogController() {
            _blogService = new BlogService();
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
        public async Task<IActionResult> CrearPost(Post post)
        {

            if (ModelState.IsValid)
            {
                await _blogService.CreatePost(post);
                return RedirectToAction("Index");
            }
            return View(post);
        }
    }
}
