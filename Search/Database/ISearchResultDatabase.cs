
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Search.Domain.Models;

namespace Search.Database
{
    public interface ISearchResultDatabase
    {
        IQueryable<SearchResultItem> GetAll();
        Task<IEnumerable<SearchResultItem>> InsertResultAsync(IEnumerable<SearchResultItem> resultItems);
    }
}
