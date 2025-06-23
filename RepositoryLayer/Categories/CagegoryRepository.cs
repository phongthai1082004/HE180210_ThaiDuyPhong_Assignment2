using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data;
using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Categories
{
    public class CagegoryRepository(FunewsManagementAsm02v1Context dbContext) : ICategoryRepository
    {

        public async Task<IEnumerable> ListAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            // Auto increment ID logic
            var lastCategory = await dbContext.Categories.OrderByDescending(c => c.CategoryId).FirstOrDefaultAsync();

            if (lastCategory != null)
            {
                category.CategoryId = (short)(lastCategory.CategoryId + 1);
            }
            else
            {
                category.CategoryId = 1; // Start from 1 if no categories exist
            }
            var addedCategory = await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return addedCategory.Entity;
        }

        public async Task<int?> DeleteAsycn(Category? category)
        {
            if (category == null)
            {
                return null;
            }
            var systemCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

            if (systemCategory == null)
            {
                return null;
            }
            dbContext.Categories.Remove(systemCategory);
            int effectedRow = await dbContext.SaveChangesAsync();
            return effectedRow;
        }


        public async Task<int?> UpdateAsync(Category category)
        {
            if (category == null)
            {
                return null;
            }
            var updatedCategory = await GetCategoryByIdAsync(category.CategoryId);
            if (updatedCategory == null)
            {
                return null;
            }

            if (category.CategoryName != null && category.CategoryName != updatedCategory.CategoryName)
            {
                updatedCategory.CategoryName = category.CategoryName;
            }

            if (category.CategoryDesciption != null && category.CategoryDesciption != updatedCategory.CategoryDesciption)
            {
                updatedCategory.CategoryDesciption = category.CategoryDesciption;
            }

            if (category.IsActive != updatedCategory.IsActive)
                updatedCategory.IsActive = category.IsActive;

            if (category.ParentCategoryId != updatedCategory.ParentCategoryId)
            {
                updatedCategory.ParentCategoryId =
                    category.ParentCategoryId == 0 ? null : category.ParentCategoryId;
            }

            dbContext.Categories.Update(updatedCategory);
            var effectedRow = await dbContext.SaveChangesAsync();
            return effectedRow;
        }

        public async Task<PaginationList<Category>> GetCategoriesAsync(int pageNumber, int pageSize, string? searchTerm, string? sortOrder)
        {
            var CategoryList = dbContext.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {

                searchTerm = searchTerm.Trim();

                CategoryList = CategoryList.Where(c => c.CategoryName.Contains(searchTerm));
            }

            CategoryList = sortOrder switch
            {
                "name" => CategoryList.OrderBy(c => c.CategoryName),
                "name_desc" => CategoryList.OrderByDescending(c => c.CategoryName),
                "description" => CategoryList.OrderBy(c => c.CategoryDesciption),
                "description_desc" => CategoryList.OrderByDescending(c => c.CategoryDesciption),
                _ => CategoryList.OrderBy(c => c.CategoryId), // Default sort by CategoryId
            };

            return await PaginationList<Category>.CreateAsync(CategoryList, pageNumber, pageSize);

        }
    }
}
