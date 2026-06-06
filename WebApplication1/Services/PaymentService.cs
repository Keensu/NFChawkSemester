using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models.Entities;
using NFChawk.Models.Enums;
using NFChawk.Models.Interfaces;


namespace NFChawk.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private const decimal CommissionRate = 0.05m;
        private readonly IConfiguration _configuration;

        public PaymentService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
        }

        public async Task ProcessAuctionPaymentAsync(Auction auction)
        {
            if (auction.WinnerId == null) return;

            string serverWalletEmail = _configuration["PlatformSettings:ServerWalletEmail"] ?? throw new Exception("Server wallet email not configured");

            var winner = await _context.Users.FirstAsync(x => x.Id == auction.WinnerId);
            var seller = await _context.Users.FirstAsync(x => x.Id == auction.SellerId);
            var serverWallet = await _context.Users.FirstOrDefaultAsync(x => x.Email == serverWalletEmail)
                             ?? throw new Exception("Server wallet not found");

            decimal price = auction.CurrentPrice;
            decimal commission = price * CommissionRate;
            decimal sellerAmount = price - commission;


            winner.HeldBalance -= price;
            winner.Balance -= price;

            seller.Balance += sellerAmount;
            serverWallet.Balance += commission;


            auction.NFT.OwnerId = winner.Id;
            auction.NFT.Status = NFTStatus.IsNotForSale;

            var now = DateTime.UtcNow;
            _context.Transactions.AddRange(
                new Transaction { UserId = winner.Id, Amount = price, Type = TransactionType.AuctionWin, CreatedAt = now, AuctionId = auction.Id },
                new Transaction { UserId = seller.Id, Amount = sellerAmount, Type = TransactionType.NFTSale, CreatedAt = now, AuctionId = auction.Id },
                new Transaction { UserId = serverWallet.Id, Amount = commission, Type = TransactionType.PlatformCommission, CreatedAt = now, AuctionId = auction.Id }
            );
        }
    }
}