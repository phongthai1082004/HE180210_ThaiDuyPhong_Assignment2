using System;
using System.Collections.Generic;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Entities;

public partial class NewsTag
{
    public string NewsArticleId { get; set; } = null!;

    public int TagId { get; set; }

    public virtual NewsArticle NewsArticle { get; set; } = null!;
}
