using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubApp1.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Comments { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public string? UserId { get; set; } // Foreign key to the user who made the comment

        [Required]
        public int PostId { get; set; } // Foreign key to the related post

        [ForeignKey("PostId")]
        public virtual Post? Post { get; set; } // Navigation property to the Post
    }
}
