using Microsoft.EntityFrameworkCore;

namespace PaymentService.Infrastructure.Persistence;

public class PaymentDBContent(IConfiguration configuration) : DbContext
{
	override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
	}
	
	public DbSet<Domain.Entities.Payment> Payment { get; set; }

	override protected void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Domain.Entities.Payment>()
			.HasIndex(p => p.PaymentId)
			.IsUnique();
	}
}