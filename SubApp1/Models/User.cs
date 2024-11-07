using System;
namespace SubApp1.Models
{
	public class User
	{
        internal readonly string? Id;

        public int UserId { get; set; }
		public required string Username { get; set; }
		public required string Email { get; set; }
		public required string Passord { get; set; }
		public required string Passord2 { get; set; }
	}
}