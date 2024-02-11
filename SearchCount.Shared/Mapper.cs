using SearchCount.Shared.DbModels;
using SearchCount.Shared.Models;

namespace SearchCount.Shared
{
    public static class Mapper
    {
        public static SearchCountHistory Map(DbSearchCountHistory dbSearchCountHistory)
        {
            if (Enum.TryParse(dbSearchCountHistory.SearchEngine, out SearchEngine searchEngine))
            {
                return new SearchCountHistory
                {
                    Id = dbSearchCountHistory.Id,
                    Url = dbSearchCountHistory.Url,
                    SearchTerm = dbSearchCountHistory.SearchTerm,
                    SearchEngine = searchEngine,
                    Indices = dbSearchCountHistory.Indices.Split(',').Select(int.Parse),
                    DateOfExcecution = dbSearchCountHistory.DateOfExcecution
                };
            }
            else
            {
                throw new InvalidCastException($"Invalid Search Engine: {dbSearchCountHistory.SearchEngine}");
            }
        }

        public static DbSearchCountHistory Map(SearchCountHistory searchCountHistory)
        {
            return new DbSearchCountHistory
            {
                Id = searchCountHistory.Id,
                Url = searchCountHistory.Url,
                SearchTerm = searchCountHistory.SearchTerm,
                SearchEngine = searchCountHistory.SearchEngine.ToString(),
                Indices = string.Join(",", searchCountHistory.Indices.Select(i => i.ToString())),
                DateOfExcecution = searchCountHistory.DateOfExcecution
            };
        }

        public static SearchCountHistory Map(SearchCountRequest searchCountRequest, IEnumerable<int> indices)
        {
            return new SearchCountHistory
            {
                Id = Guid.NewGuid(),
                Url = searchCountRequest.Url,
                SearchTerm = searchCountRequest.SearchTerm,
                SearchEngine = searchCountRequest.SearchEngine,
                Indices = indices,
                DateOfExcecution = DateTimeOffset.Now
            };
        }
    }
}
