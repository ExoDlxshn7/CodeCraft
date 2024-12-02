using Microsoft.AspNetCore.Identity;
namespace subapp2.Models
{
	public class User : IdentityUser
	{
		public string? ProfilePic { get; set; }
	}
}