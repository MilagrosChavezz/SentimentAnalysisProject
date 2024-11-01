using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SentimentAnalysis.Service;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using SentimentAnalysis.Data.Entities;

namespace SentimentAnalysis.Web.Controllers
{
    public class StatisticsController : Controller
    {

        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {

            _statisticsService = statisticsService;

        }

        public async Task<IActionResult> GetStatistics(string? genre)
        {
            var comments = await _statisticsService.GetDepressivePosts();
            var genres = comments.Select(c => c.User.Genre).Distinct().ToList();
            ViewBag.posts = new SelectList(genres); 

            if (!string.IsNullOrEmpty(genre))
            {
                comments = await _statisticsService.GetPostsByGender(genre);
            }

            return View(comments);
        }
        public async Task<IActionResult> GenderStatisticsImage()
        {
            var malePercentage = await _statisticsService.GetMalePercentage();
            var femalePercentage = await _statisticsService.GetFemalePercentage();

            // Depuración
            Console.WriteLine($"Male Percentage: {malePercentage}");
            Console.WriteLine($"Female Percentage: {femalePercentage}");

            if (malePercentage < 0 || femalePercentage < 0 ||
                (malePercentage + femalePercentage > 100))
            {
                return Content("No hay datos disponibles para mostrar el gráfico.");
            }

            try
            {
                using (var bmp = new Bitmap(400, 400))
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    var font = new System.Drawing.Font("Arial", 12);
                    g.DrawString("Gender Distribution", font, Brushes.Black, new PointF(10, 10));

                    // Dibujar el gráfico de torta
                    float startAngle = 0;
                    float maleAngle = (float)(malePercentage / 100) * 360;
                    float femaleAngle = (float)(femalePercentage / 100) * 360;

                    // Dibujar el segmento de hombres
                    g.FillPie(Brushes.Blue, 50, 50, 300, 300, startAngle, maleAngle);
                    float maleTextAngle = startAngle + (maleAngle / 2);
                    float maleX = 200 + (float)(150 * Math.Cos(maleTextAngle * Math.PI / 180));
                    float maleY = 200 + (float)(150 * Math.Sin(maleTextAngle * Math.PI / 180));
                    g.DrawString($"Male: {malePercentage:F1}%", font, Brushes.Black, maleX, maleY);

                    startAngle += maleAngle;

                    // Dibujar el segmento de mujeres
                    g.FillPie(Brushes.Red, 50, 50, 300, 300, startAngle, femaleAngle);
                    float femaleTextAngle = startAngle + (femaleAngle / 2);
                    float femaleX = 200 + (float)(150 * Math.Cos(femaleTextAngle * Math.PI / 180));
                    float femaleY = 200 + (float)(150 * Math.Sin(femaleTextAngle * Math.PI / 180));
                    g.DrawString($"Female: {femalePercentage:F1}%", font, Brushes.Black, femaleX, femaleY);

                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Png);
                        return File(ms.ToArray(), "image/png");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el gráfico: {ex.Message}");
                return Content("Ocurrió un error al generar el gráfico.");
            }
        }
    }

    }

