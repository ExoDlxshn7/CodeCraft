using Microsoft.AspNetCore.Identity;
namespace SubApp2.Models
{
	public class User : IdentityUser
	{
		public string? ProfilePic { get; set; }
	}
}