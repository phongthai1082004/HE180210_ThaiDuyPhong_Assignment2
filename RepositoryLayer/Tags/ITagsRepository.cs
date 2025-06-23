using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data;
using HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Entities;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Tags
{
    public interface ITagsRepository : IGenericRepository<Tag>
    {
        public Task<ICollection<Tag>> GetTagsByIdsAsync(IEnumerable<int> articleDtoTagIds);
    }
}
