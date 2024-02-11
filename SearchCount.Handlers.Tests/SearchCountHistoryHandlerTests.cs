using SearchCount.Shared.Models;
using SearchCount.Repositories.Interfaces;
using Moq;

namespace SearchCount.Handlers.Tests
{
    public class SearchCountHistoryHandlerTests
    {
        private readonly Mock<ISearchCountHistoryRepository> _mockSearchCountHistoryRepository = new();
        private readonly SearchCountHistoryHandler _handler;

        public SearchCountHistoryHandlerTests()
        {
            _handler = new SearchCountHistoryHandler(_mockSearchCountHistoryRepository.Object);
        }

        [Fact]
        public void GetAllSearchCountHistory_ReturnsExpectedResult()
        {
            _mockSearchCountHistoryRepository.Setup(repo => repo.GetAllSearchCountHistory()).Returns(new List<SearchCountHistory>());

            var result = _handler.GetAllSearchCountHistory();

            Assert.NotNull(result);
            _mockSearchCountHistoryRepository.Verify(repo => repo.GetAllSearchCountHistory(), Times.Once);
        }

        [Fact]
        public void GetAllSearchCountHistory_CallsRepository()
        {
            var result = _handler.GetAllSearchCountHistory();

            _mockSearchCountHistoryRepository.Verify(repo => repo.GetAllSearchCountHistory(), Times.Once);
        }
    }
}