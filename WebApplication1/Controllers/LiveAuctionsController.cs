using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models.ViewModels;

namespace NFChawk.Controllers
{
    public class LiveAuctionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        // GET: LiveAuctionsController
        public LiveAuctionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var auctions = await _context.Auctions
                .Include(a => a.NFT)
                .Include(a => a.Seller)
                .Where(a => !a.IsFinished && a.EndTime > DateTime.UtcNow)
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            var model = new LiveAuctionsViewModel
            {
                Auctions = auctions
            };

            return View(model);
        }

    }
}
