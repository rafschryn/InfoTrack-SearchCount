using Newtonsoft.Json;
using SearchCount.Shared.DbModels;
using SearchCount.Shared.Models;

namespace SearchCount.IntegrationTests.Tests
{
    public class SearchCountHistoryIntegrationTests : IClassFixture<SearchCountWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DbSearchCountHistory _searchCountHistory1;
        private readonly DbSearchCountHistory _searchCountHistory2;
        public SearchCountHistoryIntegrationTests(SearchCountWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _searchCountHistory1 = factory._searchCountHistory1;
            _searchCountHistory2 = factory._searchCountHistory2;
        }

        [Fact]
        public async Task GetAllSearchCountHistory_ReturnsOkResult_WithHistory()
        {
            var requestUri = "/history/all";

            var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<SearchCountHistory>>(stringResponse)?.ToList();

            Assert.NotNull(result);
            Assert.Contains(result, r => r.Id ==
                _searchCountHistory1.Id &&
                r.SearchEngine.ToString() == _searchCountHistory1.SearchEngine &&
                string.Join(",", r.Indices.Select(i => i.ToString())) == _searchCountHistory1.Indices &&
                r.Url == _searchCountHistory1.Url &&
                r.SearchTerm == _searchCountHistory1.SearchTerm);
            Assert.Contains(result, r => r.Id ==
                _searchCountHistory2.Id &&
                r.SearchEngine.ToString() == _searchCountHistory2.SearchEngine &&
                string.Join(",", r.Indices.Select(i => i.ToString())) == _searchCountHistory2.Indices &&
                r.Url == _searchCountHistory2.Url &&
                r.SearchTerm == _searchCountHistory2.SearchTerm);
        }
    }
}