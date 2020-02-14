using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Search.Database;
using Search.Domain.Models;
using Search.Domain.Models.SearchEngines;

namespace Search.Domain
{
    public class SearchService : ISearchService
    {
        private readonly ISearchResultDatabase database;
        public SearchService (ISearchResultDatabase _database)
        {
            database=_database;
        }
        public async Task<IEnumerable<SearchResultItem>> SearchInEnginesAsync(string searchQuery)
        {
            var searches = new List<ISearchEngine>
            {
                new GoogleSearch(),
                new BingSearch()
            };
            var responses = new List<Tuple<string, ISearchEngine>>(searches.Count);
            using (Barrier barrier = new Barrier(searches.Count))
            {
                using (Barrier finalBarrier = new Barrier(2))
                {
                    var threads = searches.Select(s => new
                    {
                        thread = new Thread(StartSearchEngine),
                        args = new SearchArgs()
                        {
                            searchEngine = s,
                            searchQuery = searchQuery,
                            barrier = barrier,
                            finalBarrier = finalBarrier,
                            responses = responses
                        }
                    }
                    );
                    foreach(var thread in threads)
                    {
                        thread.thread.Start(thread.args);
                    }

                    finalBarrier.SignalAndWait();


                }
            }
           var resultItems =  responses[0].Item2.ParseHtmlData(responses[0].Item1);
            if(resultItems.Count() > 10)
            {
                resultItems = resultItems.Take(10);
            }

            foreach(var item in resultItems)
            {
                item.Request = searchQuery;
            }


            resultItems = await database.InsertResultAsync(resultItems);

            return resultItems;
          
        }


        public IEnumerable<SearchResultItem> LocalSearch(string searchQuery)
        {
            var results = database.GetAll().Where(r => r.Title.Contains(searchQuery)).OrderBy(r => r.Position).ToList();
            return results;
        }

        private void StartSearchEngine(object args)
        {
            if(args is SearchArgs searchArgs)
            {
                searchArgs.barrier.SignalAndWait();
                var task = searchArgs.searchEngine.GetHtmlDataAsync(searchArgs.searchQuery);
                task.Wait();
                var response = task.Result;
                lock (searchArgs.barrier)
                {
                    searchArgs.responses.Add(Tuple.Create(response, searchArgs.searchEngine));
                    if (searchArgs.responses.Count == 1)
                    {
                        searchArgs.finalBarrier.SignalAndWait();
                    }

                }
            }
        }
    }
    class SearchArgs
    {
        public ISearchEngine searchEngine;
        public string searchQuery;
        public Barrier barrier;
        public Barrier finalBarrier;
        public List<Tuple<string, ISearchEngine>> responses;
    }
}
