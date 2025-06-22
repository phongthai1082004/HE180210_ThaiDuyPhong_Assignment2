using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data;
using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Accounts
{
    public class AccountRepository(FunewsManagementAsm02v1Context dbContext) : IAccountRepository
    {

        public Task<SystemAccount?> GetAccountByEmailAsync(string email)
        {
            return dbContext.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public Task<SystemAccount?> GetAccountByIdAsync(int id)
        {
            return dbContext.SystemAccounts.FirstOrDefaultAsync(a => a.AccountId == id);
        }


        public async Task<SystemAccount> CreateAsync(SystemAccount account)
        {
            // Đặt mật khẩu mặc định
            account.AccountPassword = "@1";

            // Lấy tài khoản có AccountId lớn nhất
            var lastAccount = await dbContext.SystemAccounts
                .OrderByDescending(a => a.AccountId)
                .FirstOrDefaultAsync();

            // Sinh AccountId tiếp theo
            if (lastAccount != null)
            {
                account.AccountId = (short)(lastAccount.AccountId + 1);
            }
            else
            {
                account.AccountId = 1;
            }

            // Thêm vào DbSet
            var addedAccount = dbContext.SystemAccounts.Add(account);

            // Lưu thay đổi
            await dbContext.SaveChangesAsync();

            // Trả về tài khoản mới
            return addedAccount.Entity;
        }

        public async Task<int?> DeleteAsycn(SystemAccount? account)
        {
            if (account == null)
            {
                return null;
            }
            var systemAccount = await GetAccountByIdAsync(account.AccountId);
            if (systemAccount == null)
            {
                return null;
            }

            dbContext.SystemAccounts.Remove(systemAccount);
            var effectedRow = await dbContext.SaveChangesAsync();
            return effectedRow;
        }



        public async Task<PaginationList<SystemAccount>?> GetAccountsQuery(int pageNumber, int pageSize, string? searchString, string? sortOrder)
        {
            var systemAccounts = dbContext.SystemAccounts.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();

                systemAccounts = systemAccounts.Where(a =>
                   (a.AccountName ?? "").ToLower().Contains(searchString) ||
                   (a.AccountEmail ?? "").ToLower().Contains(searchString));
            }

            systemAccounts = sortOrder switch
            {
                "name" => systemAccounts.OrderBy(a => a.AccountName),
                "name_desc" => systemAccounts.OrderByDescending(a => a.AccountName),
                "email" => systemAccounts.OrderBy(a => a.AccountEmail),
                "email_desc" => systemAccounts.OrderByDescending(a => a.AccountEmail),
                "role" => systemAccounts.OrderBy(a => a.AccountRole),
                "role_desc" => systemAccounts.OrderByDescending(a => a.AccountRole),
                _ => systemAccounts.OrderByDescending(a => a.AccountId),
            };

            var result = await PaginationList<SystemAccount>.CreateAsync(
                systemAccounts.AsNoTracking(),
                pageNumber,
                pageSize
            );
            return result;
        }

        public async Task<IEnumerable> ListAllAsync()
        {
            return await dbContext.SystemAccounts.ToListAsync();
        }

        public async Task<int?> UpdateAsync(SystemAccount account)
        {
            var systemAccount = await GetAccountByIdAsync(account.AccountId);
            if (systemAccount == null)
            {
                return null;
            }

            // Update fields that are different
            if (
                systemAccount.AccountName != account.AccountName
                && !string.IsNullOrEmpty(account.AccountName)
            )
            {
                systemAccount.AccountName = account.AccountName;
            }
            if (
                systemAccount.AccountEmail != account.AccountEmail
                && !string.IsNullOrEmpty(account.AccountEmail)
            )
            {
                var existed = await GetAccountByEmailAsync(account.AccountEmail);
                if (existed != null && existed.AccountId != account.AccountId)
                {
                    return null;
                }
                systemAccount.AccountEmail = account.AccountEmail;
            }
            if (systemAccount.AccountRole != account.AccountRole)
            {
                systemAccount.AccountRole = account.AccountRole;
            }
            if (
                systemAccount.AccountPassword != account.AccountPassword
                && !string.IsNullOrEmpty(account.AccountPassword)
            )
            {
                systemAccount.AccountPassword = account.AccountPassword;
            }

            var effectedRow = await dbContext.SaveChangesAsync();
            return effectedRow;
        }


    }
}
