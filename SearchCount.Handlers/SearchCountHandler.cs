using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SearchCount.Handlers.Interfaces;
using SearchCount.Handlers.Services;
using SearchCount.Repositories.Interfaces;
using SearchCount.Shared;
using SearchCount.Shared.Models;
using System.Text.RegularExpressions;

namespace SearchCount.Handlers
{
    public class SearchCountHandler : ISearchCountHandler
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SearchCountHandler> _logger;
        private readonly string _searchResultCount;
        private readonly ISearchCountHistoryRepository _searchCountHistoryRepository;

        public SearchCountHandler(HttpClient httpClient, ILogger<SearchCountHandler> logger, IConfiguration configuration, ISearchCountHistoryRepository searchCountHistoryRepository)
        {
            _httpClient = httpClient;
            _logger = logger;
            _searchResultCount = configuration.GetSection("SearchResultCount").Value ?? Constants.DefaultSearchResultCount;
            _searchCountHistoryRepository = searchCountHistoryRepository;
        }

        #region Process SearchCount
        public async Task<IEnumerable<int>> GetSearchCountAsync(SearchCountRequest searchCountRequest)
        {
            string searchUrl = SearchEngineService.GetUrl(searchCountRequest.SearchTerm, _searchResultCount, searchCountRequest.SearchEngine);
            SearchEngineService.AddHeaders(searchCountRequest.SearchEngine, _httpClient);

            var indices = new List<int>();
            try
            {
                var response = await _httpClient.GetStringAsync(searchUrl);
                indices = ParseSearchResults(response, searchCountRequest.Url, SearchEngineService.GetRegex(searchCountRequest.SearchEngine)).ToList();

                _logger.LogInformation($"Search done on \"{searchCountRequest.SearchEngine}\" with search term: \"{searchCountRequest.SearchTerm}\" and url: \"{searchCountRequest.Url}\"");
            }
            catch (Exception ex)
            {
                throw new SearchEngineException("Something went wrong while with the search engine", ex);
            }

            indices = indices.Count != 0 ? indices : [0];
            AddSearchCountToDb(searchCountRequest, indices);

            return indices.Count != 0 ? indices : [0];
        }
        #endregion

        #region Priavtes
        private void AddSearchCountToDb(SearchCountRequest searchCountRequest, IEnumerable<int> indices)
        {
            _searchCountHistoryRepository.AddSearchCountHistory(Mapper.Map(searchCountRequest, indices));
        }

        private static IEnumerable<int> ParseSearchResults(string htmlContent, string targetUrl, string regex)
        {
            var indices = new List<int>();

            var matches = Regex.Matches(htmlContent, regex);

            int rank = 1;
            foreach (Match match in matches)
            {
                if (match.Groups[0].Value.Contains(targetUrl))
                {
                    indices.Add(rank);
                }
                rank++;
            }

            return indices;
        }
        #endregion
    }
}
