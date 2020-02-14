using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Search.Models;
using Search.Domain;

namespace Search.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchService searchService;
        public HomeController (ISearchService _searchService)
        {
            searchService = _searchService;
        } 

        public async Task<IActionResult> Index (string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return View("Index");
            }
            var result = await searchService.SearchInEnginesAsync(searchQuery);

            var resultModel = new SearchResult()
            {
                SearchResults = result,
                SearchQuery = searchQuery
            };
            return View("SearchResults",resultModel);
        }


        public IActionResult LocalSearch(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return View("Index");
            }
            var result = searchService.LocalSearch(searchQuery);
            var resultModel = new SearchResult()
            {
                SearchResults = result,
                SearchQuery = searchQuery
            };
            return View("SearchResults", resultModel);
        }
  
    }
}
