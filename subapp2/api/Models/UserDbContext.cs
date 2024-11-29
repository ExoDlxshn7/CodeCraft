using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SubApp2.Models;

public class UserDbContext : IdentityDbContext<User>
{
	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
	{
              Database.EnsureCreated();

	}

	public new DbSet<User>? Users { get; set; }
	public DbSet<Post> Posts { get; set; }
	public DbSet<Comment> Comments { get; set; }
}