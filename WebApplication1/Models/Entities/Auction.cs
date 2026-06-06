using System.ComponentModel.DataAnnotations;

namespace NFChawk.Models.Entities
{
    public class Auction
    {
        public int Id { get; set; }

        public int NFTId { get; set; }
        public NFT NFT { get; set; }

        public int SellerId { get; set; }
        public User Seller { get; set; }

        public decimal StartPrice { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal MinBidStep { get; set; } = 1;

        public int? WinnerId { get; set; }
        public User? Winner { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsFinished { get; set; }

        public bool IsLocked { get; set; }

        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}