using Microsoft.EntityFrameworkCore;
using Unified.Connectors.EntityModels;

namespace Unified.Connectors.DBContext
{
    public class UnifiedDbContext : DbContext
    {
        public UnifiedDbContext(DbContextOptions<UnifiedDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientConfiguration> ClientConfigurations { get; set; }

    }
}
