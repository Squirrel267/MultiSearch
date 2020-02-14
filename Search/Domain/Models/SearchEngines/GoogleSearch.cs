using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace Search.Domain.Models.SearchEngines
{
    public class GoogleSearch: ISearchEngine
    {
        public  async Task<string> GetHtmlDataAsync(string searchQuery)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add(
                "user-agent",
                "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0");
             
            var response =await client.GetAsync(
                "https://www.google.com/search?q=" + HttpUtility.UrlEncode(searchQuery));
            var result = await response.Content.ReadAsStringAsync();
            return result;

        }
        public IEnumerable<SearchResultItem> ParseHtmlData(string htmlData)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlData);
            var results = doc.DocumentNode.SelectNodes(".//*[@class='js client-js']");
            List<SearchResultItem> mappedResults = results.Select((result, index) =>
            {
                var link = result.Attributes["href"].Value;

                var titleElement = result.SelectSingleNode(".//h3");
                var title = HttpUtility.HtmlDecode(titleElement.InnerText);
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
