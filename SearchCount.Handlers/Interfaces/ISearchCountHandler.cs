using SearchCount.Shared.Models;

namespace SearchCount.Handlers.Interfaces
{
    public interface ISearchCountHandler
    {
        Task<IEnumerable<int>> GetSearchCountAsync(SearchCountRequest searchCountRequest);
    }
}
