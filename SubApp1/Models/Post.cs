using System;
using System.ComponentModel.DataAnnotations; 
using SubApp1.Models;


public class Post
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }

    public string? ImageUrl { get; set; }


    // If you want a relationship with the User model
    public User? User { get; set; }
}
