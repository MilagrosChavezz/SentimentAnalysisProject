using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SentimentAnalysis.Data.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace SentimentAnalysis.Service
{
    public interface IStatisticsService
    {
        Task<List<Post>> GetDepressivePosts();
        Task<double> GetFemalePercentage();
        Task<double> GetMalePercentage();
        Task<int> GetTotalUsers();
        Task<int> GetTotalUsersFemale();
        Task<int> GetTotalUsersMale();
        Task<List<Post>> GetPostsByGender(string gender);

    }
    public class StatisticsService : IStatisticsService
    {
        private readonly SentimentAnalysisContext context;

        public StatisticsService()
        {
            context = new SentimentAnalysisContext();
        }

        public async Task<List<Post>> GetDepressivePosts()
        {
            return await context.Posts
                                 .Where(p => p.IsDepression == true)
                                 .Include(p => p.User)
                                 .ToListAsync();
        }

        public async Task<double> GetFemalePercentage()
        {
            int totalPosts = await context.Posts.CountAsync(p => p.IsDepression == true);
            if (totalPosts == 0) return 0;

            int femalePosts = await context.Posts.CountAsync(p => p.IsDepression == true && p.User.Genre == "female");
            return (double)femalePosts / totalPosts * 100;
        }


        public async Task<double> GetMalePercentage()
        {
            int totalPosts = await context.Posts.CountAsync(p => p.IsDepression == true);
            if (totalPosts == 0) return 0;

            int malePosts = await context.Posts.CountAsync(p => p.IsDepression == true && p.User.Genre == "male");
            return (double)malePosts / totalPosts * 100;
        }


        public Task<int> GetTotalUsers()
        {
            return context.Posts
                          .Where(p => p.IsDepression == true)
                          .Select(p => p.UserId)
                          .Distinct()
                          .CountAsync();
        }

        public async Task<int> GetTotalUsersFemale()
        {
            return await context.Posts
                         .Where(p => p.IsDepression == true && p.User.Genre == "female")
                         .Select(p => p.UserId)
                         .Distinct()
                         .CountAsync();
        }

        public async Task<int> GetTotalUsersMale()
        {
            return await context.Posts
                         .Where(p => p.IsDepression == true && p.User.Genre == "male")
                         .Select(p => p.UserId)
                         .Distinct()
                         .CountAsync();
        }

        public async Task<List<Post>> GetPostsByGender(string gender)
        {
            return await context.Posts
                                 .Where(p => p.IsDepression == true && p.User.Genre == gender)
                                 .Include(p => p.User)
                                 .ToListAsync();
        }
    }
}
