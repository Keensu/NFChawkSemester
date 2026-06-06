using NFChawk.Models.ViewModels;

namespace NFChawk.Models.Interfaces
{
    public interface INFTService
    {
        Task<int> CreateNFTAsync(CreateNFTViewModel model, int authorId);
        Task AssignToCollectionAsync(int nftId, int? collectionId);
        Task DeleteNFTAsync(int id);
        Task PutOnSaleAsync(int id, int currentUserId);
        Task CancelSaleAsync(int id);
        Task BuyAsync(int id, int currentUserId);
        Task UpdateDetailsAsync(int id, string type, string name, string description, decimal? price, int currentUserId);
    }
}
