using NFChawk.Models.Enums;

namespace NFChawk.Models.Entities
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public ActivityType Type { get; set; }
        public int? NFTId { get; set; }
        public NFT? NFT { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
