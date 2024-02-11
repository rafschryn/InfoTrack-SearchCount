using Microsoft.EntityFrameworkCore;
using SearchCount.Shared.DbModels;

namespace SearchCount.Contexts
{
    public class SearchCountContext : DbContext
    {
        public SearchCountContext(DbContextOptions<SearchCountContext> options)
            : base(options)
        {
        }

        public DbSet<DbSearchCountHistory> SearchCountHistory { get; set; }
    }
}