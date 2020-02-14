using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Search.Domain.Models;

namespace Search.Domain
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResultItem>> SearchInEnginesAsync(string searchQuery);
        IEnumerable<SearchResultItem> LocalSearch(string searchQuery);

    }
}
