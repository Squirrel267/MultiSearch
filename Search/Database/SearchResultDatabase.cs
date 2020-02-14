using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Search.Domain.Models;

namespace Search.Database
{
    public class SearchResultDatabase :ISearchResultDatabase
    {
        private readonly SearchContext context;
        public SearchResultDatabase (SearchContext _context)
        {
            context = _context;
        }
        public IQueryable<SearchResultItem> GetAll()
        {
            //not cached
            return context.SearchResults.AsNoTracking();

        }
        public async Task<IEnumerable<SearchResultItem>> InsertResultAsync(IEnumerable<SearchResultItem> resultItems)
        {
            await context.SearchResults.AddRangeAsync(resultItems);
            await context.SaveChangesAsync();

            return resultItems;
        }

    }
}
