using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NFChawk.Models.DTO;
using NFChawk.Models.Entities;
using NFChawk.Models.Interfaces;

namespace NFChawk.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionService _auctionService;

        private readonly UserManager<User> _userManager;

        public AuctionController(
            IAuctionService auctionService,
            UserManager<User> userManager)
        {
            _auctionService = auctionService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(int nftId)
        {
            var model = new CreateAuctionDto
            {
                NFTId = nftId,
                EndTime = DateTime.UtcNow.AddHours(24)
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAuctionDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                var user = await _userManager.GetUserAsync(User);

                await _auctionService.CreateAuctionAsync(user.Id, dto);

                return RedirectToAction("Index", "ItemDetails", new { id = dto.NFTId });
            }
            catch (Exception ex)
            {
                var realMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                ModelState.AddModelError("", realMessage);

                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var auction = await _auctionService
                .GetAuctionByIdAsync(id);

            if (auction == null)
                return NotFound();

            var bids = await _auctionService
                .GetAuctionBidsAsync(id);

            ViewBag.Bids = bids;

            return View(auction);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceBid(int auctionId, PlaceBidDto dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                await _auctionService.PlaceBidAsync(
                    auctionId,
                    user.Id,
                    dto.Amount);

                return RedirectToAction(
                    "Details",
                    new { id = auctionId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(
                    "Details",
                    new { id = auctionId });
            }

        }
    }
}
