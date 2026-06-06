using NFChawk.Models.Entities;

namespace NFChawk.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<NFT> FeaturedNfts { get; set; } = new List<NFT>();
        public List<Collection> PopularCollections { get; set; } = new List<Collection>();
        public List<Auction> LiveAuctions { get; set; } = new List<Auction>();
        public List<NFT> NewestItems { get; set; } = new List<NFT>();
    }
}
