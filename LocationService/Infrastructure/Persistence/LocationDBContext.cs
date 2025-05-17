using LocationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Infrastructure.Persistence;

public class LocationDBContext(IConfiguration configuration) : DbContext
{
	override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
	}
	
	public DbSet<Location> Location { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Location>()
			.HasIndex(l => l.LocationId)
			.IsUnique();
	}
}
