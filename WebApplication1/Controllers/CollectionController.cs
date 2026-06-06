using Microsoft.AspNetCore.Mvc;
using NFChawk.Models.ViewModels;
using NFChawk.Models.Interfaces;
using System.Security.Claims;

namespace NFChawk.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public async Task<IActionResult> Index()
        {
            var collections = await _collectionService.GetAllCollectionsAsync();

            return View(collections);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCollectionViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            
            var userId = int.Parse(userIdString);

            await _collectionService.CreateCollectionAsync(model, userId);

            return RedirectToAction("Index", "Personal");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _collectionService.DeleteCollectionAsync(id);
            return RedirectToAction("Index", "Personal");
        }
    }
}