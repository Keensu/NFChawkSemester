using NFChawk.Models.Entities;

namespace NFChawk.Models.ViewModels
{
    public class ItemDetailsViewModel
    {
        public NFT NFT { get; set; }
        public Collection? Collection { get; set; }

        public string Name => NFT?.Name ?? Collection?.Name ?? "Unknown";
        public string Description => NFT?.Description ?? Collection?.Description ?? "";
        public string Image => NFT?.ImageUrl ?? Collection?.BannerImageUrl ?? "";
        public string AuthorName => NFT?.Author?.UserName ?? Collection?.Author?.UserName ?? "Anonymous";
    }
}
