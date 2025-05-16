using TicketService.Domain.Entities;

namespace TicketService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class TicketDBContext(IConfiguration configuration) : DbContext
{
	override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
	}
	
	public DbSet<Ticket> Ticket { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Ticket>()
			.HasIndex(t => t.BookingId)
			.IsUnique();
	}

}