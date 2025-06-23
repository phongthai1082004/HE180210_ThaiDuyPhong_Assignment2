using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data;
using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.NewsArticles
{
    public class NewsArticles(FunewsManagementAsm02v1Context dbContext) : INewsArticles
    {
        public async Task<NewsArticle>? CreateAsync(NewsArticle newArticle)
        {
            if (newArticle == null)
            {
                return null;
            }

            var lastArticle = await dbContext.NewsArticles
                .OrderByDescending(a => a.CreatedDate)
                .FirstOrDefaultAsync();

            if (lastArticle != null)
            {
                newArticle.NewsArticleId = (lastArticle.CreatedDate?.Ticks + 1).ToString() ?? "1"; // Generate a unique ID based on the last article's CreatedDate
            }
            else
            {
                newArticle.NewsArticleId = "1";
            }

            var addedArticle = await dbContext.NewsArticles.AddAsync(newArticle);
            await dbContext.SaveChangesAsync();
            return addedArticle.Entity;
        }

        public async Task<int?> DeleteAsycn(NewsArticle? newArticle)
        {
            if (newArticle == null)
            {
                return null;
            }

            var systemArticle = await dbContext.NewsArticles.FirstOrDefaultAsync(a => a.NewsArticleId == newArticle.NewsArticleId);
            if (systemArticle == null)
            {
                return null;
            }
            dbContext.NewsArticles.Remove(systemArticle);
            int effectedRow = await dbContext.SaveChangesAsync();
            return effectedRow;
        }

        public async Task<NewsArticle?> GetArticleByIdAsync(string id)
        {
            return await dbContext.NewsArticles.FirstOrDefaultAsync(a => a.NewsArticleId == id);
        }

        public async Task<PaginationList<NewsArticle>> GetCategoriesQuery(int pageNumber, int pageSize, string? searchString, string? sortOrder)
        {
            // Bắt đầu với IQueryable
            IQueryable<NewsArticle> query = dbContext.NewsArticles;

            // Lọc theo từ khóa nếu có
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();
                query = query.Where(a =>
                    a.NewsTitle.ToLower().Contains(searchString) ||
                    a.Headline.ToLower().Contains(searchString) ||
                    a.NewsContent.ToLower().Contains(searchString) ||
                    a.NewsSource.ToLower().Contains(searchString) ||
                    a.CreatedBy.AccountName.ToLower().Contains(searchString));
            }

            // Sắp xếp
            query = sortOrder switch
            {
                "title" => query.OrderBy(a => a.NewsTitle),
                "title_desc" => query.OrderByDescending(a => a.NewsTitle),
                "date_asc" => query.OrderBy(a => a.CreatedDate),
                "date_desc" => query.OrderByDescending(a => a.CreatedDate),
                _ => query.OrderBy(a => a.NewsTitle), // mặc định
            };

            // Phân trang
            var result = await PaginationList<NewsArticle>.CreateAsync(query, pageNumber, pageSize);
            return result;
        }


        public async Task<IEnumerable> ListAllAsync()
        {
            return await dbContext.NewsArticles.ToListAsync();
        }

        public async Task<int?> UpdateAsync(NewsArticle newsArticle)
        {
            if (newsArticle == null)
            {
                return null;
            }

            var existingArticle = await dbContext.NewsArticles.FirstOrDefaultAsync(a => a.NewsArticleId == newsArticle.NewsArticleId);
            if (existingArticle == null)
            {
                return null;
            }
            existingArticle.NewsTitle = newsArticle.NewsTitle;
            existingArticle.Headline = newsArticle.Headline;
            existingArticle.NewsContent = newsArticle.NewsContent;
            existingArticle.NewsSource = newsArticle.NewsSource;
            existingArticle.CategoryId = newsArticle.CategoryId;
            existingArticle.NewsStatus = newsArticle.NewsStatus;
            existingArticle.UpdatedById = newsArticle.UpdatedById;
            existingArticle.ModifiedDate = DateTime.UtcNow;

            // handle Tag
            var existingTags = existingArticle.Tags.ToList();
            foreach (var tag in existingTags)
            {
                if (newsArticle.Tags.All(t => t.TagId != tag.TagId))
                {
                    existingArticle.Tags.Remove(tag);
                }
            }

            foreach (var tag in newsArticle.Tags)
            {
                if (existingArticle.Tags.All(t => t.TagId != tag.TagId))
                {
                    existingArticle.Tags.Add(tag);
                }
            }


            dbContext.NewsArticles.Update(existingArticle);
            var effectedRow = await dbContext.SaveChangesAsync();
            return effectedRow;
        }
    }
}
