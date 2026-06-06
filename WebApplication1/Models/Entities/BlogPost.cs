using System.ComponentModel.DataAnnotations;

namespace NFChawk.Models.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; } 

        public string ImageUrl { get; set; } 

        public string Category { get; set; } 

        public string CategoryDisplay { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
