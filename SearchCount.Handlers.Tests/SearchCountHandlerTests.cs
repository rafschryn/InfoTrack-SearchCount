using SearchCount.Shared.Models;
using SearchCount.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SearchCount.Repositories.Interfaces;
using System.Net;
using Moq;
using Moq.Protected;
using System.Net.Http.Headers;

namespace SearchCount.Handlers.Tests
{
    public class SearchCountHandlerTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new(MockBehavior.Strict);
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<SearchCountHandler>> _mockLogger = new();
        private readonly Mock<IConfiguration> _mockConfiguration = new();
        private readonly Mock<ISearchCountHistoryRepository> _mockSearchCountHistoryRepository = new();
        private readonly SearchCountHandler _handler;

        public SearchCountHandlerTests()
        {
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockConfiguration.Setup(c => c.GetSection("SearchResultCount").Value).Returns("100");

            _handler = new SearchCountHandler(_httpClient, _mockLogger.Object, _mockConfiguration.Object, _mockSearchCountHistoryRepository.Object);
        }

        [Theory]
        [InlineData("example.com", @"<div class=""egMi0 kCrYT""><a href=""/url?q=example.com""", 1)]
        [InlineData("example.com", @"<div class=""egMi0 kCrYT""><a href=""/url?q=example.com""" +
            "---<div class=\"egMi0 kCrYT\"><a href=\"/url?q=FAKE.com\"" +
            "---<div class=\"egMi0 kCrYT\"><a href=\"/url?q=example.com\"", 1, 3)]
        public async Task GetSearchCountAsync_ReturnsIndices(string url, string http, params int[] expectedCount)
        {
            var request = new SearchCountRequest
            {
                SearchTerm = "test",
                Url = url,
                SearchEngine = SearchEngine.Google
            };
            var responseContent = new StringContent(http);
            _mockHttpMessageHandler.SetupRequest()
                .ReturnsResponse(responseContent, "text/plain");

            var result = await _handler.GetSearchCountAsync(request);

            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public async Task GetSearchCountAsync_CallsRepository()
        {
            var request = new SearchCountRequest
            {
                SearchTerm = "test",
                Url = "example.com",
                SearchEngine = SearchEngine.Google
            };
            var responseContent = new StringContent(string.Empty);
            _mockHttpMessageHandler.SetupRequest()
                .ReturnsResponse(responseContent, "text/html");

            var result = await _handler.GetSearchCountAsync(request);

            _mockSearchCountHistoryRepository.Verify(r => r.AddSearchCountHistory(It.IsAny<SearchCountHistory>()), Times.Once);
        }


        [Fact]
        public async Task GetSearchCountAsync_Logs()
        {
            var request = new SearchCountRequest
            {
                SearchTerm = "test",
                Url = "http://example.com",
                SearchEngine = SearchEngine.Google
            };
            var responseContent = new StringContent("<html></html>");
            _mockHttpMessageHandler.SetupRequest()
                .ReturnsResponse(responseContent, "text/html");

            var result = await _handler.GetSearchCountAsync(request);

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _mockLogger.Verify(l =>
            l.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(
                    $"Search done on \"{request.SearchEngine}\" with search term: \"{request.SearchTerm}\" and url: \"{request.Url}\"")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        }

        [Fact]
        public async Task GetSearchCountAsync_WhenExceptionThrown_LogsAndRethrows()
        {
            var request = new SearchCountRequest
            {
                SearchTerm = "test",
                Url = "http://example.com",
                SearchEngine = SearchEngine.Google
            };
            _mockHttpMessageHandler.SetupRequestToThrow();

            await Assert.ThrowsAsync<SearchEngineException>(() => _handler.GetSearchCountAsync(request));
        }
    }

    public static class MockHttpMessageHandlerExtensions
    {
        public static Mock<HttpMessageHandler> SetupRequestToThrow(this Mock<HttpMessageHandler> mockHandler)
        {
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).Throws(new HttpRequestException("Network error"));

            return mockHandler;
        }

        public static Mock<HttpMessageHandler> SetupRequest(this Mock<HttpMessageHandler> mockHandler)
        {
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            return mockHandler;
        }

        public static Mock<HttpMessageHandler> ReturnsResponse(this Mock<HttpMessageHandler> mockHandler, HttpContent content, string mediaType)
        {
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = content
                })
                .Callback<HttpRequestMessage, CancellationToken>((req, ct) => req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType)))
                .Verifiable();

            return mockHandler;
        }
    }
}