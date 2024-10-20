using System;
using System.Collections.Generic;

namespace SentimentAnalysis.Data.Entities;

public partial class Post
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int UserId { get; set; }

    public bool? IsDepression { get; set; }

    public DateOnly PostDate { get; set; }

    public virtual User User { get; set; } = null!;
}
