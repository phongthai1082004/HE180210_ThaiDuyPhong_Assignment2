using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data;
using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Entities;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Accounts
{
    public interface IAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount?> GetAccountByIdAsync(int id);
        Task<SystemAccount?> GetAccountByEmailAsync(string email);

        Task<PaginationList<SystemAccount>?> GetAccountsQuery(int pageNumber, int pageSize, string? searchString, string? sortOrder);
    }
}
