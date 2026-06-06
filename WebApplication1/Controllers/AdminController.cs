using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models.Entities;
using NFChawk.Models.Enums;
using NFChawk.Models.ViewModels;

namespace NFChawk.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;

        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            if (_context == null)
            {
                _logger.LogError("Database context is null.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Database context is not available.");
            }
            var viewModel = new AdminViewModel
            {
                Users = await _context.Users.ToListAsync(),
                BlogPosts = await _context.BlogPosts.ToListAsync(),
                Collections = await _context.Collections.ToListAsync(),
                NFTs = await _context.NFTs.ToListAsync(),
                Transactions = await _context.Transactions.ToListAsync(),
                Auctions = await _context.Auctions.ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, User model)
        {
            if (id != model.Id)
            {
                return RedirectToAction("Error404", "Error");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction("Error404", "Error");
            }

            if (ModelState.IsValid)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Balance = model.Balance;
                user.HeldBalance = model.HeldBalance;

                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult CreateBlogPost()
        {
            return View("UpsertBlogPost", new BlogPost());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost(BlogPost blogPost, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog", fileName);

                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    blogPost.ImageUrl = "/images/blog/" + fileName;
                }
                else
                {
                    blogPost.ImageUrl = "/images/800x800.png";
                }

                blogPost.CreatedAt = DateTime.Now;
                _context.BlogPosts.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("UpsertBlogPost", blogPost);
        }

        [HttpGet]
        public async Task<IActionResult> EditBlogPost(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            return View("UpsertBlogPost", blogPost);
        }

        [HttpPost]
        public async Task<IActionResult> EditBlogPost(int id, BlogPost blogPost, IFormFile? ImageFile)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog", fileName);

                        var directory = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        if (!string.IsNullOrEmpty(blogPost.ImageUrl) && !blogPost.ImageUrl.Contains("placeholder.png"))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blogPost.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        blogPost.ImageUrl = "/images/blog/" + fileName;
                    }

                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BlogPosts.Any(e => e.Id == blogPost.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View("UpsertBlogPost", blogPost);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNFT(int id)
        {
            var nft = await _context.NFTs.FindAsync(id);
            if (nft == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            _context.NFTs.Remove(nft);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAuction(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
            {
                return RedirectToAction("Error404", "Error");
            }

            if (auction.IsFinished)
            {
                _logger.LogWarning($"Attempt to cancel already finished auction {id}");
                return RedirectToAction("Index");
            }

            using (var dbTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var highestBid = auction.Bids.OrderByDescending(b => b.Amount).FirstOrDefault();

                    if (highestBid != null)
                    {
                        var bidder = await _context.Users.FindAsync(highestBid.BidderId);
                        if (bidder != null)
                        {
                            bidder.Balance += highestBid.Amount;

                            var refundTransaction = new Transaction
                            {
                                UserId = bidder.Id,
                                Amount = highestBid.Amount,
                                Type = TransactionType.Deposit,
                                Description = $"Refund for auction cancellation #{auction.Id} (NFT ID: {auction.NFTId})",
                                CreatedAt = DateTime.Now,
                                NFTId = auction.NFTId,
                                AuctionId = auction.Id
                            };

                            _context.Transactions.Add(refundTransaction);
                        }
                    }

                    auction.IsFinished = true;
                    auction.IsLocked = true;

                    _context.Update(auction);
                    await _context.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await dbTransaction.RollbackAsync();
                    _logger.LogError(ex, $"Error while canceling auction {id}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return RedirectToAction("Error404", "Error");
            }

            var user = await _context.Users.FindAsync(transaction.UserId);
            if (user == null)
            {
                return RedirectToAction("Error404", "Error");
            }

            using (var dbTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    decimal refundAmount = 0;
                    string actionText = "";

                    switch (transaction.Type)
                    {
                        case TransactionType.Withdraw:
                        case TransactionType.NFTPurchase:
                        case TransactionType.AuctionBid:
                            refundAmount = transaction.Amount;
                            actionText = "Cancellation of the debit";
                            break;

                        case TransactionType.Deposit:
                        case TransactionType.NFTSale:
                        case TransactionType.AuctionWin:
                            refundAmount = -transaction.Amount;
                            actionText = "Cancellation of the credit";
                            break;

                        case TransactionType.PlatformCommission:
                            refundAmount = transaction.Amount;
                            actionText = "Cancellation of the commission";
                            break;
                    }

                    user.Balance += refundAmount;

                    var cancellationLog = new Transaction
                    {
                        UserId = user.Id,
                        Amount = Math.Abs(refundAmount),
                        Type = refundAmount > 0 ? TransactionType.Deposit : TransactionType.Withdraw,
                        Description = $"{actionText} for transaction #{transaction.Id} ({transaction.Description})",
                        CreatedAt = DateTime.Now,
                        NFTId = transaction.NFTId,
                        AuctionId = transaction.AuctionId
                    };

                    _context.Transactions.Add(cancellationLog);

                    transaction.Description += " [Canceled by Admin]";
                    _context.Update(transaction);

                    await _context.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await dbTransaction.RollbackAsync();
                    _logger.LogError(ex, $"Error while canceling transaction {id}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
