using NFChawk.Models.Entities;

namespace NFChawk.Models.ViewModels
{
    public class AdminViewModel
    {
        public List<User> Users { get; set; } = new();
        public List<BlogPost> BlogPosts { get; set; } = new();
        public List<Collection> Collections { get; set; } = new();
        public List<NFT> NFTs { get; set; } = new();
        public List<Transaction> Transactions { get; set; } = new();
        public List<Auction> Auctions { get; set; } = new();

    }
}
