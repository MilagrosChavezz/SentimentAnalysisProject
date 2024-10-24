
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using System;
using System.Threading.Tasks;

namespace SentimentAnalysis.Service
{
    public class PeopleService
    {
        public async Task<(DateOnly? birthday, string? gender)> GetUserProfile(string accessToken)
        {
            // Configurar el servicio con el token de acceso
            var credential = GoogleCredential.FromAccessToken(accessToken);
            var service = new PeopleServiceService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "authentication",
            });

            // Hacer la solicitud para obtener la información de la persona
            var peopleRequest = service.People.Get("people/me");
            peopleRequest.PersonFields = "birthdays,genders";
            Person profile = await peopleRequest.ExecuteAsync();

            // Procesar los resultados
            var birthday = profile.Birthdays?[0]?.Date;
         

            DateOnly? birthdayResult = null;
            if (birthday != null)
            {
                birthdayResult = new DateOnly(birthday.Year ?? 0, birthday.Month ?? 0, birthday.Day ?? 0);
            }

            var gender = profile.Genders?[0]?.Value;
            Console.WriteLine($"Género: {gender}");

            return (birthdayResult, gender);
        }
    }
}
