using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public interface IPagedList<T> : IList<T>
    {
        int CurrentPage { get; }
        bool HasNext { get; }
        bool HasPrevious { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }

        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, int count)
        {
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}