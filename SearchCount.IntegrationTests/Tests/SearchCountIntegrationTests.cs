using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SearchCount.Contexts;
using SearchCount.Shared.DbModels;
using SearchCount.Shared.Models;

namespace SearchCount.IntegrationTests
{
    public class SearchCountIntegrationTests : IClassFixture<SearchCountWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly SearchCountWebApplicationFactory<Program> _factory;

        public SearchCountIntegrationTests(SearchCountWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllSearchCountHistory_ReturnsOkResult_WithHistory()
        {
            var requestUri = "/search/count";
            var request = new SearchCountRequest
            {
                SearchTerm = "InfoTrack",
                Url = "infotrack.co.uk",
                SearchEngine = SearchEngine.Google
            };

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(requestUri, httpContent);
            response.EnsureSuccessStatusCode();

            using var _scope = _factory.Services.CreateScope();
            var mockDbContext = _scope.ServiceProvider.GetRequiredService<SearchCountContext>();
            var dbSearchCountHistory = mockDbContext.SearchCountHistory.Single(h =>
                h.SearchEngine == request.SearchEngine.ToString() &&
                h.SearchTerm == request.SearchTerm &&
                h.Url == request.Url);

            Assert.NotNull(dbSearchCountHistory);
            Assert.NotEmpty(dbSearchCountHistory.Indices);
        }
    }
}