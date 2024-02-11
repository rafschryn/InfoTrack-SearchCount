using SearchCount.Handlers.Interfaces;
using SearchCount.Repositories.Interfaces;
using SearchCount.Shared.Models;

namespace SearchCount.Handlers
{
    public class SearchCountHistoryHandler : ISearchCountHistoryHandler
    {
        private readonly ISearchCountHistoryRepository _searchCountHistoryRepository;

        public SearchCountHistoryHandler(ISearchCountHistoryRepository searchCountHistoryRepository)
        {
            _searchCountHistoryRepository = searchCountHistoryRepository;
        }

        public IEnumerable<SearchCountHistory> GetAllSearchCountHistory()
        {
            return _searchCountHistoryRepository.GetAllSearchCountHistory();
        }
    }
}
