using NFChawk.Models.Enums;

namespace NFChawk.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? NFTId { get; set; }

        public int? AuctionId { get; set; }
    }
}
