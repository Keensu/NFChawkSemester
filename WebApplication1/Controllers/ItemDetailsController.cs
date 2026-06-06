using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models;
using NFChawk.Models.ViewModels;

namespace NFChawk.Controllers
{
    public class ItemDetailsController : Controller
    {
        private readonly ApplicationDbContext _context; 

        public ItemDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /ItemDetails/Index/5?type=nft
        public async Task<IActionResult> Index(int id, string type)
        {
            var viewModel = new ItemDetailsViewModel();
            if (type == "nft")
            {
                viewModel.NFT = await _context.NFTs
                    .Include(n => n.Author)
                    .Include(n => n.Collection)
                    .FirstOrDefaultAsync(n => n.Id == id);

                if (viewModel.NFT == null) return NotFound();
            }
            else if (type == "collection")
            {
                viewModel.Collection = await _context.Collections
                    .Include(c => c.Author)
                    .Include(c => c.NFTs)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (viewModel.Collection == null) return NotFound();
            }

            return View(viewModel);
        }
    }
}
