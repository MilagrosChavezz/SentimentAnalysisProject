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



    }
}
