using HotelListing.API.Data;
using HotelListing.API.Data.Models;
using HotelListing.API.EF.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HotelListing.API.EF
{
    public class PostGreContext : IdentityDbContext<ApiUser>
    {
        public PostGreContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
        }

    }

    public class PostGreContextFactory : IDesignTimeDbContextFactory<PostGreContext>
    {
        public PostGreContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PostGreContext>();
            var connectionString = configuration.GetConnectionString("PostGresConnectionString");
            optionsBuilder.UseNpgsql(connectionString);

            return new PostGreContext(optionsBuilder.Options);
        }
    }
}
