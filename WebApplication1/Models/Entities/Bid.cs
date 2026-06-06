namespace NFChawk.Models.Entities
{
    public class Bid
    {
        public int Id { get; set; }

        public int AuctionId { get; set; }
        public Auction Auction { get; set; }

        public int BidderId { get; set; }
        public User Bidder { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
