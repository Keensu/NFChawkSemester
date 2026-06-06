using NFChawk.Models.Entities;

namespace NFChawk.Models.Interfaces
{
    public interface IPaymentService
    {
        Task ProcessAuctionPaymentAsync(Auction auction);
    }
}
