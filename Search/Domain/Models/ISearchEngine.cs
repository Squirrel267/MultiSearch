using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Search.Domain.Models
{
    public interface ISearchEngine
    {
        Task<string> GetHtmlDataAsync(string searchQuery);

        IEnumerable<SearchResultItem> ParseHtmlData(string htmlData);

    }
}
