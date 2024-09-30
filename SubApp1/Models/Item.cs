using System;
namespace SubApp1.Models
{
	public class Item
	{
		public int ItemId { get; set; }
		public string Name { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
	}
}