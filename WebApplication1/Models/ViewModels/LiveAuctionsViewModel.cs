using NFChawk.Models.Entities;

namespace NFChawk.Models.ViewModels
{
    public class LiveAuctionsViewModel
    {
        public List<Auction> Auctions { get; set; } = new List<Auction>();
    }
}
