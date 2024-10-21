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
    public class UserService
    {
        private readonly SentimentAnalysisContext context;


        public UserService()
        {
            context = new SentimentAnalysisContext();
        }

        public async Task<List<User>> GetUsers()
        {
            return await context.Users.ToListAsync();
        }


        public async Task CreateUserIfNotExists(string email, string fullName, DateTime? birthday, string gender, string country)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser == null)
            {
                var newUser = new User
                {
                    Email = email,
                    UserName = fullName,
                    Birthday = birthday.HasValue ? DateOnly.FromDateTime(birthday.Value) : (DateOnly?)null,
                    Genre = gender,
                    Country = country
                };
                context.Users.Add(newUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
