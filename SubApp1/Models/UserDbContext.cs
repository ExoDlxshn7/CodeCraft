using Microsoft.EntityFrameworkCore;

namespace SubApp1.Models;

public class UserDbContext : DbContext
{
	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
	{
       Database.EnsureCreated();
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Friend> Friends { get; set; }
	public DbSet<Post> Posts { get; set; }
}