using Microsoft.EntityFrameworkCore;
using SearchCount.Contexts;
using SearchCount.Shared.DbModels;
using SearchCount.Shared.Models;

namespace SearchCount.Repositories.Tests
{
    public class SearchCountHistoryRepositoryTests
    {
        private readonly DbContextOptions<SearchCountContext> _options;
        private readonly DbSearchCountHistory SearchCountHistory1 = new DbSearchCountHistory
        {
            Id = Guid.NewGuid(),
            Url = "Url1",
            SearchEngine = SearchEngine.Google.ToString()
        };
        private readonly DbSearchCountHistory SearchCountHistory2 = new DbSearchCountHistory
        {
            Id = Guid.NewGuid(),
            Url = "Url2",
            SearchEngine = SearchEngine.Bing.ToString()
        };

        public SearchCountHistoryRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<SearchCountContext>()
                .UseInMemoryDatabase(databaseName: "SearchCountTestDb")
                .Options;
            SetupDb();
        }

        private void SetupDb()
        {
            using var context = new SearchCountContext(_options);
            if (!context.SearchCountHistory.Any())
            {
                context.SearchCountHistory.AddRange(SearchCountHistory1, SearchCountHistory2);
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetAllSearchCountHistory_ReturnsAllRecords()
        {
            using var context = new SearchCountContext(_options);
            var repository = new SearchCountHistoryRepository(context);

            var historyRecords = repository.GetAllSearchCountHistory();

            Assert.Equal(context.SearchCountHistory.Count(), historyRecords.Count());
        }

        [Fact]
        public void AddSearchCountHistory_AddsSuccessfully()
        {
            using var context = new SearchCountContext(_options);
            var repository = new SearchCountHistoryRepository(context);
            var newHistory = new SearchCountHistory
            {
                Id = Guid.NewGuid(),
                Url = "Url3",
                SearchEngine = SearchEngine.Google
            };
            var count = context.SearchCountHistory.Count();

            repository.AddSearchCountHistory(newHistory);

            Assert.Equal(count + 1, context.SearchCountHistory.Count());
            Assert.Equal(newHistory.Id, context.SearchCountHistory.ToList()[count].Id);
        }
    }
}