using System.ComponentModel.DataAnnotations;

namespace SentimentAnalysis.Entitys
{
    public class Data
    {
       
        public string clean_text { get; set; }
        public bool is_depression { get; set; }
    }
}
