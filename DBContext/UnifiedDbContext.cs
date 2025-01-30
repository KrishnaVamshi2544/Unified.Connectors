using Microsoft.EntityFrameworkCore;
using Unified.Connectors.EntityModels;

namespace Unified.Connectors.DBContext
{
    /// <summary>
    /// 
    /// </summary>
    public class UnifiedDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public UnifiedDbContext(DbContextOptions<UnifiedDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Connector> Connectors { get; set; }
        public DbSet<CompanyConfiguration> CompanyConfigurations { get; set; }
        public DbSet<JobQueue> JobQueues { get; set; }

    }
}
