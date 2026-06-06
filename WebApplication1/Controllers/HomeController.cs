using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using NFChawk.Models; 
using NFChawk.Models.Entities;
using NFChawk.Data;
using NFChawk.Models.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; 


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string pageToOpen = "Index";
            var viewModel = new HomeViewModel();

            
            viewModel.FeaturedNfts = await _context.NFTs
                .Include(n => n.Author)
                .Include(n => n.Owner)
                .Include(n => n.Auction)
                .Take(5)
                .ToListAsync();

            
            viewModel.LiveAuctions = await _context.Auctions
                .Include(a => a.NFT)
                .Include(a => a.Seller)
                .Where(a => a.EndTime > DateTime.UtcNow && !a.IsFinished)
                .OrderByDescending(a => a.StartTime) 
                .Take(6)
                .ToListAsync();


            viewModel.NewestItems = await _context.NFTs
                .Include(n => n.Author)
                .Include(n => n.Owner)
                .Include(n => n.Auction)
                .OrderByDescending(n => n.Id)
                .Take(6)
                .ToListAsync();

            
            viewModel.PopularCollections = await _context.Collections
                .Include(c => c.NFTs)
                .Take(3)
                .ToListAsync();

            return View(pageToOpen, viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}