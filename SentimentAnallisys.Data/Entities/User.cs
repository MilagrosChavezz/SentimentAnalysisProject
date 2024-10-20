﻿using System;
using System.Collections.Generic;

namespace SentimentAnalysis.Data.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Country { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Genre { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
