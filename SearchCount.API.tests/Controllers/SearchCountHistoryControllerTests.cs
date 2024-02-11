using Moq;
using Microsoft.AspNetCore.Mvc;
using SearchCount.Handlers.Interfaces;
using SearchCount.API.Controllers;
using SearchCount.Shared.Models;


namespace SearchCount.API.Tests.Controllers
{
    public class SearchCountHistoryControllerTests
    {
        private readonly Mock<ISearchCountHistoryHandler> _mockSearchCountHistoryHandler = new();
        private readonly SearchCountHistoryController _controller;

        public SearchCountHistoryControllerTests()
        {
            _mockSearchCountHistoryHandler.Setup(h => h.GetAllSearchCountHistory()).Returns(new List<SearchCountHistory>());

            _controller = new SearchCountHistoryController(_mockSearchCountHistoryHandler.Object);
        }

        [Fact]
        public void GetAllSearchCountHistory_ReturnsExpectedResult()
        {
            var actionResult = _controller.GetAllSearchCountHistory();

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var resultValue = Assert.IsType<List<SearchCountHistory>>(okResult.Value);
        }

        [Fact]
        public void GetAllSearchCountHistory_CallsHandler()
        {
            _mockSearchCountHistoryHandler.Setup(h => h.GetAllSearchCountHistory()).Returns(new List<SearchCountHistory>());

            _controller.GetAllSearchCountHistory();

            _mockSearchCountHistoryHandler.Verify(handler => handler.GetAllSearchCountHistory(), Times.Once);
        }
    }
}
