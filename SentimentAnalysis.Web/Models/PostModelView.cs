using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SentimentAnalysis.Web.Models
{
    public class PostModelView
    {
        public string Text { get; set; } 

        public string Title { get; set; } 

        public bool? IsDepression { get; set; }

        public DateOnly? PostDate { get; set; }

   


    }
}
