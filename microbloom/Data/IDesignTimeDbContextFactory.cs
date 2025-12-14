using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace microbloom.Data 
{
    public class KariyerDbContextFactory : IDesignTimeDbContextFactory<KariyerDBContext>
    {
        public KariyerDBContext CreateDbContext(string[] args)
        {
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            

            var optionsBuilder = new DbContextOptionsBuilder<KariyerDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new KariyerDBContext(optionsBuilder.Options);
        }
    }
}