using Microsoft.AspNetCore.Identity;

namespace NFChawk.Models.Entities
{
    public class User : IdentityUser<int>
    {
        public decimal Balance { get; set; } = 2000;

        public decimal HeldBalance { get; set; } = 0;

        public List<Collection> Collections { get; set; } = new();
        public List<NFT> NFTs { get; set; } = new();

        public List<Bid> Bids { get; set; } = new();

        public List<Auction> CreatedAuctions { get; set; } = new();
        public List<Transaction> Transactions { get; set; } = new();

    }
}
