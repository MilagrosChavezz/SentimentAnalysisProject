using Microsoft.ML;
using Microsoft.ML.Data;
using SentimentAnalysis.Entitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SentimentAnalysis.Data;
using SentimentAnalysis.Data.Entities;

using Microsoft.EntityFrameworkCore;


namespace SentimentAnalysis.Service
{
    public class BlogService
    {
        private readonly SentimentAnalysisContext context;

        public BlogService()
        {
            context = new SentimentAnalysisContext();
        }

        public async Task<List<Post>> GetPosts()
        {
            return await context.Posts.ToListAsync();
        }

        public async Task<Post?> GetPostById(int id)
        {
            return await context.Posts.FindAsync(id);
        }

        public async Task<Post> CreatePost(Post post)
        {
            post.PostDate = DateOnly.FromDateTime(DateTime.Now);
            context.Posts.Add(post);
            await context.SaveChangesAsync();
            return post;
        }
    }
    }

 
