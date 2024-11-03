using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SentimentAnalysis.Service;
using SentimentAnalysis.Entitys;
using Microsoft.ML;
using Microsoft.AspNetCore.Authorization;


namespace SentimentAnalysis.Web.Controllers
{
    [AllowAnonymous]
    public class PredictionController : Controller
    {
        private readonly MLAnalysis service;

        public PredictionController(MLAnalysis service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult Predict(SentimentAnalysis.Entitys.Data Data)
        {
            if (ModelState.IsValid)
            {
                var prediction = service.Predict(Data);

                if (prediction.Score >= 0.5)
                {
                    TempData["PredictionResult"] = $"Suicidal (Score: {prediction.Score})";
                }
                else
                {
                    TempData["PredictionResult"] = $"Not Suicidal (Score: {prediction.Score})";
                }

                return RedirectToAction("Predict");
            }

            return View(Data);
        }
     
    }
}
