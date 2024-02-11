using SearchCount.Shared;
using SearchCount.Shared.Models;
using System.Web;

namespace SearchCount.Handlers.Services
{
    public static class SearchEngineService
    {
        public static string GetUrl(string searchTerm, string resultCount, SearchEngine searchEngine)
        {
            return searchEngine switch
            {
                SearchEngine.Google => $"http://www.google.co.uk/search?q={HttpUtility.UrlEncode(searchTerm)}&num={resultCount}",
                SearchEngine.Bing => $"http://www.bing.com/search?q={HttpUtility.UrlEncode(searchTerm)}&count={resultCount}",
                _ => throw new CaseNotHandledException("Search Engin implementation missing")
            };
        }

        public static string GetRegex(SearchEngine searchEngine)
        {
            return searchEngine switch
            {
                SearchEngine.Google => Constants.GoogleRegex,
                SearchEngine.Bing => Constants.BingRegex,
                _ => throw new CaseNotHandledException("Search Engin implementation missing")
            };
        }

        public static void AddHeaders(SearchEngine searchEngine, HttpClient httpClient)
        {
            switch (searchEngine)
            {
                case SearchEngine.Bing:
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36");
                    return;

                default: return;
            }
        }
    }
}
