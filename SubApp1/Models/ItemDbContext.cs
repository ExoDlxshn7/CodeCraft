using Microsoft.EntityFrameworkCore;

namespace SubApp1.Models;

public class ItemDbContext : DbContext
{
	public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
	{
        Database.EnsureCreated();
	}

	public DbSet<Item> Items { get; set; }
}