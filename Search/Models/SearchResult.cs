using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Search.Domain.Models;
namespace Search.Models
{
    public class SearchResult
    {
        public IEnumerable<SearchResultItem> SearchResults { get; set; }
        public string SearchQuery { get; set; }
    }
}
