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


namespace SentimentAnalysis.Service
{
    public class MLAnalysis
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public MLAnalysis()
        {
            _mlContext = new MLContext();

            var dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "depression_dataset.csv");

            if (!File.Exists(dataPath))
            {
                throw new FileNotFoundException($"File not found at {dataPath}");
            }

            var loader = _mlContext.Data.CreateTextLoader(new TextLoader.Options
            {
                Separators = new[] { ',' },
                HasHeader = true,
                Columns = new[]
                {
                        new TextLoader.Column("clean_text", DataKind.String, 0),
                        new TextLoader.Column("is_depression", DataKind.Boolean, 1),

                    }
            });
            IDataView dataView = loader.Load(dataPath);

            var preview = dataView.Preview();
            if (preview.RowView.Length == 0)
            {
                throw new Exception("No data loaded");
            }

            Console.WriteLine("Loaded data:");
            foreach (var row in preview.RowView)
            {
                var columnData = row.Values.Select(column => $"{column.Key}: {column.Value}");
                Console.WriteLine(string.Join(", ", columnData));
            }

            var dataProcessPipeline = _mlContext.Transforms.Text.FeaturizeText("Features", "clean_text")
           .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
           .AppendCacheCheckpoint(_mlContext);

            var trainer = _mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "is_depression", featureColumnName: "Features");


            var trainingPipeline = dataProcessPipeline.Append(trainer);

            _model = trainingPipeline.Fit(dataView);
        }


        public Prediction Predict(SentimentAnalysis.Entitys.Data data)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentAnalysis.Entitys.Data, Prediction>(_model);

            var prediction = predictionEngine.Predict(data);

            double threshold = 0.6; // Ajusta este valor según tus necesidades

            // Modifica el valor de is_depression basado en el umbral
            prediction.is_depression = prediction.Score >= threshold;

            return prediction;
        }



    }
    }

 
