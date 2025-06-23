using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data;
using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Tags
{
    public class TagsRepository : ITagsRepository
    {

        private readonly FunewsManagementAsm02v1Context _context;

        public TagsRepository(FunewsManagementAsm02v1Context context)
        {
            _context = context;
        }


        public Task<Tag> CreateAsync(Tag entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> DeleteAsycn(Tag? entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Tag>> GetTagsByIdsAsync(IEnumerable<int> articleDtoTagIds)
        {
            var tags = await _context.Tags.Where(t => articleDtoTagIds.Contains(t.TagId)).ToListAsync();
            return tags;
        }

        public async Task<IEnumerable> ListAllAsync()
        {
            return await _context.Tags.AsNoTracking().ToListAsync();
        }

        public Task<int?> UpdateAsync(Tag entity)
        {
            throw new NotImplementedException();
        }
    }
}