using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Hubs;
using NFChawk.Models.DTO;
using NFChawk.Models.Entities;
using NFChawk.Models.Enums;
using NFChawk.Models.Interfaces;

namespace NFChawk.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AuctionHub> _hubContext;

        public AuctionService(ApplicationDbContext context, IHubContext<AuctionHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task CreateAuctionAsync(int sellerId, CreateAuctionDto dto)
        {
            var nft = await _context.NFTs.FirstOrDefaultAsync(x => x.Id == dto.NFTId);

            if (nft == null)
                throw new Exception("NFT not found");

            if (nft.OwnerId != sellerId)
                throw new Exception("Not owner");

            if (nft.Status == NFTStatus.OnAuction)
                throw new Exception("Already on auction");

            var utcEndTime = dto.EndTime.ToUniversalTime();
            if (utcEndTime <= DateTime.UtcNow)
                throw new Exception("Auction end time must be in the future");

            var auction = new Auction
            {
                NFTId = nft.Id,
                SellerId = sellerId,
                StartPrice = dto.StartPrice,
                CurrentPrice = dto.StartPrice,
                MinBidStep = dto.MinBidStep,
                StartTime = DateTime.UtcNow,
                EndTime = utcEndTime
            };

            nft.Status = NFTStatus.OnAuction;

            _context.Auctions.Add(auction);

            await _context.SaveChangesAsync();
        }

        public async Task PlaceBidAsync(int auctionId, int userId, decimal amount)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var auction = await _context.Auctions
                        .Include(a => a.Winner)
                        .FirstOrDefaultAsync(a => a.Id == auctionId);

                    if (auction == null) throw new Exception("Auction not found");
                    if (auction.IsFinished || auction.EndTime <= DateTime.UtcNow) throw new Exception("Auction has ended");
                    if (auction.IsLocked) throw new Exception("Auction is being processed, please try again later");
                    if (auction.SellerId == userId) throw new Exception("You cannot bid on your own auction");

                    var user = await _context.Users.FindAsync(userId);
                    var minBid = auction.CurrentPrice + auction.MinBidStep;

                    if (amount < minBid) throw new Exception($"Minimum bid: {minBid}");
                    if ((user.Balance - user.HeldBalance) < amount) throw new Exception("Insufficient funds");


                    if (auction.WinnerId != null)
                    {
                        auction.Winner.HeldBalance -= auction.CurrentPrice;
                    }


                    user.HeldBalance += amount;


                    auction.CurrentPrice = amount;
                    auction.WinnerId = userId;


                    if ((auction.EndTime - DateTime.UtcNow).TotalSeconds < 30)
                    {
                        auction.EndTime = DateTime.UtcNow.AddSeconds(60);
                    }

                    _context.Bids.Add(new Bid
                    {
                        AuctionId = auction.Id,
                        BidderId = userId,
                        Amount = amount,
                        CreatedAt = DateTime.UtcNow
                    });

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();


                    await _hubContext.Clients.Group($"Auction_{auctionId}").SendAsync("ReceiveBid", new
                    {
                        userName = user.UserName,
                        amount,
                        currentPrice = auction.CurrentPrice,
                        endTime = auction.EndTime
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("The price changed while you were placing your bid. Please try again.");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<Auction?> GetAuctionByIdAsync(int id)
        {
            return await _context.Auctions
                .Include(a => a.NFT)
                .Include(a => a.Seller)
                .Include(a => a.Winner)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Bid>> GetAuctionBidsAsync(int auctionId)
        {
            return await _context.Bids
                .Include(b => b.Bidder)
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }
    }
}