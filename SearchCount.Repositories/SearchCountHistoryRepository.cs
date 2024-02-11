using SearchCount.Contexts;
using SearchCount.Repositories.Interfaces;
using SearchCount.Shared;
using SearchCount.Shared.Models;

namespace SearchCount.Repositories
{
    public class SearchCountHistoryRepository : ISearchCountHistoryRepository
    {
        private readonly SearchCountContext _context;

        public SearchCountHistoryRepository(SearchCountContext context)
        {
            _context = context;
        }

        public void AddSearchCountHistory(SearchCountHistory searchCountHistory)
        {
            _context.SearchCountHistory.Add(Mapper.Map(searchCountHistory));

            _context.SaveChanges();
        }

        public IEnumerable<SearchCountHistory> GetAllSearchCountHistory()
        {
            return _context.SearchCountHistory.Select(Mapper.Map);
        }
    }
}
