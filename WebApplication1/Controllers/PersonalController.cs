using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models;
using NFChawk.Models.Entities;
using NFChawk.Models.ViewModels;

namespace NFChawk.Controllers
{
    public class PersonalController : Controller
    {
        // GET: PersonalController
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public PersonalController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("LogIn", "Auth");
            }

            var userId = user.Id;

            var myNfts = await _context.NFTs
                .Where(n => n.OwnerId == userId || n.AuthorId == userId)
                .Include(n => n.Auction)
                .ToListAsync();

            var myCollections = await _context.Collections
                .Where(x => x.AuthorId == userId || x.AuthorId == userId)
                .Include(x => x.NFTs)
                .ToListAsync();

            var model = new PersonalViewModel
            {
                User = user,
                MyNFTs = myNfts,
                MyCollections = myCollections
            };

            return View(model);
        }
    }
}
