using Microsoft.EntityFrameworkCore;

namespace SubApp1.Models;

public class RegisterDbContext : DbContext
{
	public RegisterDbContext(DbContextOptions<RegisterDbContext> options) : base(options)
	{
        Database.EnsureCreated();
	}

	public DbSet<User> Users { get; set; }
}