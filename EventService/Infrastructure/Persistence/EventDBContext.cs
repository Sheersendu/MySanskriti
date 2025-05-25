using EventService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventService.Infrastructure.Persistence;

public class EventDBContext(IConfiguration configuration) : DbContext
{
	override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
	}

	public DbSet<Event> Events { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Event>()
			.HasIndex(e => e.EventId)
			.IsUnique();
	}
}