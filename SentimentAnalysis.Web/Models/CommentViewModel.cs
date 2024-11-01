using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.Data.Entities;

namespace SentimentAnalysis.Web.Models
{
    public class CommentViewModel
    {

        public List<Post> Posts { get; set; }


    }
}

