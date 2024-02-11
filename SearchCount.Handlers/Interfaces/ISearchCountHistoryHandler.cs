using SearchCount.Shared.Models;

namespace SearchCount.Handlers.Interfaces
{
    public interface ISearchCountHistoryHandler
    {
        IEnumerable<SearchCountHistory> GetAllSearchCountHistory();
    }
}
