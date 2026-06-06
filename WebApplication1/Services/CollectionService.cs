using Microsoft.EntityFrameworkCore;
using NFChawk.Data;
using NFChawk.Models.Entities;
using NFChawk.Models.Interfaces;
using NFChawk.Models.ViewModels;
using System.IO;

namespace NFChawk.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly ApplicationDbContext _context;

        public CollectionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Collection>> GetAllCollectionsAsync()
        {
            return await _context.Collections
                .Include(x => x.NFTs)
                .ToListAsync();
        }

        public async Task<int> CreateCollectionAsync(CreateCollectionViewModel model, int userId)
        {
            string imagePath = "/images/resource/collection-1.png";

            if (model.BannerImage != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.BannerImage.FileName);
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var uploadPath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await model.BannerImage.CopyToAsync(stream);
                }
                imagePath = "/uploads/" + fileName;
            }

            var collection = new Collection
            {
                Name = model.Name,
                Description = model.Description,
                BannerImageUrl = imagePath,
                AuthorId = userId
            };

            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();

            return collection.Id;
        }

        public async Task DeleteCollectionAsync(int id)
        {
            var collection = await _context.Collections
                .Include(c => c.NFTs)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (collection != null)
            {
                foreach (var nft in collection.NFTs)
                {
                    nft.CollectionId = null;
                }

                _context.Collections.Remove(collection);
                await _context.SaveChangesAsync();
            }
        }
    }
}