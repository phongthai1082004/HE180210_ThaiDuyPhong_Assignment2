using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data;
using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Entities;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.NewsArticles
{
    public interface INewsArticles : IGenericRepository<NewsArticle>
    {
        Task<NewsArticle?> GetArticleByIdAsync(string id);
        Task<PaginationList<NewsArticle>> GetCategoriesQuery(
            int pageNumber,
            int pageSize,
            string? searchString,
            string? sortOrder
        );
    }
}
