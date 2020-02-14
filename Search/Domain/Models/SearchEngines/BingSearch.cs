using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Web;

namespace Search.Domain.Models.SearchEngines
{
    public class BingSearch:ISearchEngine

    {
        public async Task<string> GetHtmlDataAsync(string searchQuery)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add(
                "user-agent",
                "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0");
            // Bing gives search results only after setting cookies
            //Because of this reason , I executed 2 requests
            await client.GetAsync(
                "https://www.bing.com/search?q=" + HttpUtility.UrlEncode(searchQuery));
            var response = await client.GetAsync(
                "https://www.bing.com/search?q=" + HttpUtility.UrlEncode(searchQuery));

            var result = await response.Content.ReadAsStringAsync();
            return result;

        }
        public IEnumerable<SearchResultItem> ParseHtmlData(string htmlData)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlData);
            var results = doc.DocumentNode.SelectNodes(".//li[@class=\"b_algo\"]//h2/a");
            List<SearchResultItem> mappedResults = results.Select((result, index) =>
            {
                var link = result.Attributes["href"].Value;

                var title = HttpUtility.HtmlDecode(result.InnerText);

                return new SearchResultItem()
                {
                    Position = index,
                    Title = title,
                    Link = link

                };


            }).ToList();

            return mappedResults;

        }
    }
}
