using Microsoft.EntityFrameworkCore;
using Search.Domain.Models;

namespace Search.Database
{
    public class SearchContext: DbContext 
    {
        public SearchContext(DbContextOptions<SearchContext> options): base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<SearchResultItem> SearchResults { get; set; }
    }
}
