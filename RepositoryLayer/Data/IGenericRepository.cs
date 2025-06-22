using System.Collections;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable> ListAllAsync();
        Task<T> CreateAsync(T entity);
        Task<int?> UpdateAsync(T entity);
        Task<int?> DeleteAsycn(T? entity);
    }
}
