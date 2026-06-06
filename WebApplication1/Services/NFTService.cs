using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models.Entities;
using NFChawk.Models.Interfaces;
using NFChawk.Models.ViewModels;
using NFChawk.Models.Enums;

namespace NFChawk.Services
{
    public class NFTService : INFTService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailService _emailService;

        public NFTService(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            IEmailService emailService)
        {
            _context = context;
            _environment = environment;
            _emailService = emailService;
        }

        public async Task<int> CreateNFTAsync(CreateNFTViewModel model, int authorId)
        {


            if (model.ImageFile == null)
            {
                throw new Exception("ImageFile is NULL");
            }



            string webRootPath = _environment.WebRootPath;


            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot");
            }



            string uploadFolder = Path.Combine(
                webRootPath,
                "uploads",
                "nfts");


            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }



            string originalFileName =
                Path.GetFileName(model.ImageFile.FileName);

            string uniqueFileName =
                $"{Guid.NewGuid()}_{originalFileName}";


            string filePath = Path.Combine(
                uploadFolder,
                uniqueFileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }


            var nft = new NFT
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = $"/uploads/nfts/{uniqueFileName}",
                CollectionId = model.CollectionId,
                AuthorId = authorId
            };


            _context.NFTs.Add(nft);

            await _context.SaveChangesAsync();


            return nft.Id;
        }

        public async Task AssignToCollectionAsync(int nftId, int? collectionId)
        {
            var nft = await _context.NFTs.FirstOrDefaultAsync(x => x.Id == nftId);
            if (nft == null) throw new KeyNotFoundException("NFT not found");

            nft.CollectionId = collectionId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteNFTAsync(int id)
        {
            var nft = await _context.NFTs.FindAsync(id);
            if (nft != null)
            {
                _context.NFTs.Remove(nft);
                await _context.SaveChangesAsync();
            }
        }

        public async Task PutOnSaleAsync(int id, int currentUserId)
        {
            var nft = await _context.NFTs.FindAsync(id);
            if (nft == null) throw new KeyNotFoundException("NFT not found");

            bool isOwner = (nft.OwnerId == currentUserId) || (nft.OwnerId == null && nft.AuthorId == currentUserId);

            if (!isOwner)
            {
                throw new UnauthorizedAccessException("Forbidden");
            }

            nft.Status = NFTStatus.IsForSale;
            await _context.SaveChangesAsync();
        }

        public async Task CancelSaleAsync(int id)
        {
            var nft = await _context.NFTs.FindAsync(id);
            if (nft == null) throw new KeyNotFoundException("NFT not found");

            nft.Status = NFTStatus.IsNotForSale;
            await _context.SaveChangesAsync();
        }

        public async Task BuyAsync(int id, int currentUserId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var nft = await _context.NFTs.FindAsync(id);
                    if (nft == null || nft.Status != NFTStatus.IsForSale)
                        throw new Exception("NFT is not for sale or doesn't exist");

                    var currentUser = await _context.Users.FindAsync(currentUserId);
                    if (currentUser == null)
                        throw new Exception("User not found");

                    var sellerId = nft.OwnerId ?? nft.AuthorId;
                    if (sellerId == currentUserId)
                        throw new Exception("You cannot buy your own NFT");

                    var seller = await _context.Users.FindAsync(sellerId);
                    if (seller == null)
                        throw new Exception("Seller not found");

                    var availableBalance = currentUser.Balance - currentUser.HeldBalance;
                    if (availableBalance < nft.Price)
                        throw new Exception("Insufficient balance");

                    currentUser.Balance -= nft.Price;
                    seller.Balance += nft.Price;

                    nft.OwnerId = currentUserId;
                    nft.Status = NFTStatus.IsNotForSale;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    if (!string.IsNullOrEmpty(currentUser.Email))
                    {
                        await _emailService.SendEmailAsync(
                            currentUser.Email,
                            "NFT Purchased",
                            $"You bought the NFT: {nft.Name}!"
                        );
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task UpdateDetailsAsync(int id, string type, string name, string description, decimal? price, int currentUserId)
        {
            if (type == "nft")
            {
                var nft = await _context.NFTs.FindAsync(id);
                if (nft == null) throw new KeyNotFoundException("NFT not found");

                bool isOwner = nft.OwnerId == currentUserId || (nft.OwnerId == null && nft.AuthorId == currentUserId);
                bool isAuthor = nft.AuthorId == currentUserId;

                if (!isOwner) throw new UnauthorizedAccessException("Forbidden");

                if (isAuthor)
                {
                    nft.Name = name;
                    nft.Description = description;
                }
                if (price.HasValue) nft.Price = price.Value;
            }
            else if (type == "collection")
            {
                var collection = await _context.Collections.FindAsync(id);
                if (collection == null) throw new KeyNotFoundException("Collection not found");
                if (collection.AuthorId != currentUserId) throw new UnauthorizedAccessException("Forbidden");

                collection.Name = name;
                collection.Description = description;
            }

            await _context.SaveChangesAsync();
        }
    }
}