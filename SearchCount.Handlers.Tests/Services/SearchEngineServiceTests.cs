using SearchCount.Handlers.Services;
using SearchCount.Shared.Models;
using SearchCount.Shared;

namespace SearchCount.Handlers.Tests.Services
{
    public class SearchCountHandlerTests
    {
        public SearchCountHandlerTests()
        {}

        [Theory]
        [InlineData("test", "10", SearchEngine.Google, "http://www.google.co.uk/search?q=test&num=10")]
        [InlineData("test", "5", SearchEngine.Bing, "http://www.bing.com/search?q=test&count=5")]
        public void GetUrl_ReturnsExpectedUrl(string searchTerm, string resultCount, SearchEngine searchEngine, string expectedUrl)
        {
            var url = SearchEngineService.GetUrl(searchTerm, resultCount, searchEngine);

            Assert.Equal(expectedUrl, url);
        }

        [Theory]
        [InlineData(SearchEngine.Google, Constants.GoogleRegex)]
        [InlineData(SearchEngine.Bing, Constants.BingRegex)]
        public void GetRegex_ReturnsExpectedRegex(SearchEngine searchEngine, string expectedRegex)
        {
            var regex = SearchEngineService.GetRegex(searchEngine);

            Assert.Equal(expectedRegex, regex);
        }

        [Fact]
        public void AddHeaders_WhenBing_AddsUserAgent()
        {
            var client = new HttpClient();
            SearchEngineService.AddHeaders(SearchEngine.Bing, client);

            Assert.Contains("Mozilla/5.0", client.DefaultRequestHeaders.UserAgent.ToString());
        }

        [Fact]
        public void AddHeaders_WhenNotBing_DoesNotAddUserAgent()
        {
            var client = new HttpClient();
            SearchEngineService.AddHeaders(SearchEngine.Google, client);

            Assert.Empty(client.DefaultRequestHeaders.UserAgent.ToString());
        }

        [Fact]
        public void GetUrl_ThrowsCaseNotHandledExceptionForUnsupportedEngine()
        {
            var searchTerm = "test";
            var resultCount = "10";
            var searchEngine = (SearchEngine)(-1);

            Assert.Throws<CaseNotHandledException>(() => SearchEngineService.GetUrl(searchTerm, resultCount, searchEngine));
        }

        [Fact]
        public void GetRegex_ThrowsCaseNotHandledExceptionForUnsupportedEngine()
        {
            var searchEngine = (SearchEngine)(-1);

            Assert.Throws<CaseNotHandledException>(() => SearchEngineService.GetRegex(searchEngine));
        }
    }
}