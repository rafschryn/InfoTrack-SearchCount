using SearchCount.Shared.Models;

namespace SearchCount.Repositories.Interfaces
{
    public interface ISearchCountHistoryRepository
    {
        void AddSearchCountHistory(SearchCountHistory searchCountHistory);
        IEnumerable<SearchCountHistory> GetAllSearchCountHistory();
    }
}
