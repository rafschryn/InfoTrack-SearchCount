using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SearchCount.Contexts;
using SearchCount.Shared.DbModels;
using SearchCount.Shared.Models;

namespace SearchCount.IntegrationTests
{
    public class SearchCountWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public DbSearchCountHistory _searchCountHistory1;
        public DbSearchCountHistory _searchCountHistory2;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<SearchCountContext>));

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<SearchCountContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<SearchCountContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<SearchCountWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        SeedDatabase(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database with test data. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        private void SeedDatabase(SearchCountContext context)
        {
            _searchCountHistory1 = new DbSearchCountHistory
            {
                Id = Guid.NewGuid(),
                Url = "Url1",
                SearchTerm = "SearchTerm1",
                SearchEngine = SearchEngine.Google.ToString(),
                Indices = "1,2,3",
                DateOfExcecution = DateTimeOffset.Now
            };
            _searchCountHistory2 = new DbSearchCountHistory
            {
                Id = Guid.NewGuid(),
                Url = "Url2",
                SearchTerm = "SearchTerm2",
                SearchEngine = SearchEngine.Bing.ToString(),
                Indices = "4,5,6",
                DateOfExcecution = DateTimeOffset.Now
            }; 

            context.SearchCountHistory.AddRange(_searchCountHistory1, _searchCountHistory2);
            context.SaveChanges();
        }
    }
}
