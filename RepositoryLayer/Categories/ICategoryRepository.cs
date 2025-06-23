using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data;
using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Entities;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Categories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<PaginationList<Category>> GetCategoriesAsync(int pageNumber, int pageSize, string? searchTerm, string? sortOrder);
    }
}
