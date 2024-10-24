using System;
using System.Collections.Generic;

namespace SentimentAnalysis.Data.Entities;

public partial class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string Email { get; set; } = null!;
    public DateOnly? Birthday { get; set; }
    public string? Genre { get; set; }
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
