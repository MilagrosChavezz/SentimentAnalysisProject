using System.ComponentModel.DataAnnotations;

namespace SentimentAnalysis.Entitys
{
    public class Data
    {
        [Required(ErrorMessage ="You must put a message")]
        public string clean_text { get; set; }
        public bool is_depression { get; set; }
    }
}
