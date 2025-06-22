using Microsoft.EntityFrameworkCore;

namespace HE180210_ThaiDuyPhong_Assignment2.RepositoryLayer.Data
{
    public class PaginationList<T> : List<T>
    {
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public int PageIndex { get; set; }
        public bool hasPreviousPage => PageIndex > 1;
        public bool hasNextPage => PageIndex < TotalPages;


        public PaginationList() { }

        public PaginationList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            TotalElements = count;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public static async Task<PaginationList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginationList<T>(items, count, pageIndex, pageSize);
        }
    }
}
