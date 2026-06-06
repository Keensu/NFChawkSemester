using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Hubs;
using NFChawk.Models.Enums;
using NFChawk.Models.Interfaces;

namespace NFChawk.Services
{
    public class AuctionFinalizationService : IAuctionFinalizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AuctionHub> _hubContext;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuctionFinalizationService> _logger;

        public AuctionFinalizationService(
            ApplicationDbContext context,
            IHubContext<AuctionHub> hubContext,
            IPaymentService paymentService,
            IEmailService emailService,
            ILogger<AuctionFinalizationService> logger
            )
        {
            _context = context;
            _hubContext = hubContext;
            _paymentService = paymentService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task FinalizeExpiredAuctionsAsync()
        {
            var auctions = await _context.Auctions
                .Include(a => a.NFT)
                .Include(a => a.Winner)
                .Where(a => !a.IsFinished && a.EndTime <= DateTime.UtcNow)
                .ToListAsync();

            foreach (var auction in auctions)
            {
                using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    auction.IsLocked = true;
                    await _context.SaveChangesAsync();

                    if (auction.WinnerId == null)
                    {
                        auction.IsFinished = true;

                        auction.NFT.Status = NFTStatus.IsForSale;
                    }
                    else
                    {

                        await _paymentService.ProcessAuctionPaymentAsync(auction);


                        auction.NFT.OwnerId = auction.WinnerId;

                        auction.NFT.Status = NFTStatus.IsNotForSale;


                        auction.NFT.Price = auction.CurrentPrice;

                        auction.IsFinished = true;

                        if (auction.Winner != null && !string.IsNullOrEmpty(auction.Winner.Email))
                        {
                            await _emailService.SendEmailAsync(
                                auction.Winner.Email,
                                "Auction Won",
                                $"Congrats, you won the auction for {auction.NFT.Name}!");
                        }
                    }

                    auction.IsLocked = false;
                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();

                    
                    await _hubContext.Clients.Group($"Auction_{auction.Id}").SendAsync("AuctionFinished", new
                    {
                        auctionId = auction.Id,
                        winnerId = auction.WinnerId,
                        winnerName = auction.Winner?.UserName 
                    });
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    _logger.LogError(ex, $"Error finalizing auction {auction.Id}");
                }
            }
        }
    }
}