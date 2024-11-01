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
using SentimentAnalysis.Service;

using Microsoft.EntityFrameworkCore;


namespace SentimentAnalysis.Service
{
    public class UserService
    {
        private readonly SentimentAnalysisContext _context;
        private readonly PeopleService _peopleService;

        public UserService(SentimentAnalysisContext context, PeopleService peopleService)
        {
            _context = context;
           _peopleService = peopleService;
        }

      
        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        
        public async Task SaveUserProfileAsync(string email, string name, string gender, DateOnly? birthdate)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
               
                user = new User
                {
                    Email = email,
                    UserName = name,
                    Genre = gender,
                    Birthday = birthdate
                };

                _context.Users.Add(user);
            }
            else
            {
               
                user.UserName = name;
                user.Genre = gender;
                user.Birthday = birthdate;
            }

            await _context.SaveChangesAsync();
        }

     
        public async Task<User> GetUserByEmailAsync(string userEmail)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        }

        
    }
}


