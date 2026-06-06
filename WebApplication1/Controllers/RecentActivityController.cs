using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models.Enums;
using NFChawk.Models.ViewModels;

namespace NFChawk.Controllers
{
    public class RecentActivityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecentActivityController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? filter)
        {
            var query = _context.ActivityLogs
                .Include(a => a.NFT)
                .Include(a => a.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = filter.ToLower() switch
                {
                    "mint" => query.Where(a => a.Type == ActivityType.Mint),
                    "listings" => query.Where(a => a.Type == ActivityType.Listing),
                    "purchases" => query.Where(a => a.Type == ActivityType.Purchase),
                    "sales" => query.Where(a => a.Type == ActivityType.Sale),
                    "bids" => query.Where(a => a.Type == ActivityType.Bid),
                    _ => query
                };
            }

            var activities = await query
                .OrderByDescending(a => a.CreatedAt)
                .Take(20) 
                .ToListAsync();

            var model = new ActivityViewModel
            {
                Activities = activities,
                CurrentFilter = filter ?? "all"
            };

            return View(model);
        }
    }
}
