using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models.Entities;
using NFChawk.Models.Enums;
using NFChawk.Models.Interfaces;
using NFChawk.Models.ViewModels;
using System.Security.Claims;

namespace NFChawk.Controllers
{
    public class NFTController : Controller
    {
        private readonly INFTService _nftService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public NFTController(INFTService nftService, ApplicationDbContext context, UserManager<User> userManager)
        {
            _nftService = nftService;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Collections = _context.Collections.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> Create(CreateNFTViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Collections = _context.Collections.ToList();
                    return View(model);
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                    return Unauthorized();

                var userId = int.Parse(userIdClaim.Value);

                await _nftService.CreateNFTAsync(model, userId);

                return RedirectToAction("Index", "Personal");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignToCollection(int nftId, int? collectionId)
        {
            try
            {
                await _nftService.AssignToCollectionAsync(nftId, collectionId);
                return RedirectToAction("Index", "Personal");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _nftService.DeleteNFTAsync(id);
            return RedirectToAction("Index", "Personal");
        }
        
        [HttpPost]
        public async Task<IActionResult> PutOnSale(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Unauthorized();
                
                await _nftService.PutOnSaleAsync(id, currentUser.Id);
                return RedirectToAction("Index", "Personal");
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("Error404", "Error");
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Error403", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelSale(int id)
        {
            try
            {
                await _nftService.CancelSaleAsync(id);
                return RedirectToAction("Index", "Personal");
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("Error404", "Error");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Unauthorized();
                
                await _nftService.BuyAsync(id, currentUser.Id);
                TempData["SuccessMessage"] = "You have successfully purchased this NFT!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index", "ItemDetails", new { id = id, type = "nft" });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateDetails(int id, string type, string name, string description, decimal? price)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Unauthorized();
                
                await _nftService.UpdateDetailsAsync(id, type, name, description, price, currentUser.Id);
                TempData["SuccessMessage"] = "Asset updated successfully!";
                return RedirectToAction("Index", "ItemDetails", new { id = id, type = type });
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("Error404", "Error");
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Error403", "Error");
            }
        }
    }
}