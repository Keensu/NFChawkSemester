using NFChawk.Models.DTO;
using NFChawk.Models.Entities;

namespace NFChawk.Models.Interfaces
{
    public interface IAuctionService
    {
        Task CreateAuctionAsync(int sellerId, CreateAuctionDto dto);

        Task PlaceBidAsync(int auctionId, int userId, decimal amount);
        Task<Auction?> GetAuctionByIdAsync(int id);

        Task<List<Bid>> GetAuctionBidsAsync(int auctionId);
    }
}
