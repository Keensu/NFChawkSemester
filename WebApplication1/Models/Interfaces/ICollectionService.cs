using NFChawk.Models.Entities;
using NFChawk.Models.ViewModels;

namespace NFChawk.Models.Interfaces
{
    public interface ICollectionService
    {
        Task<List<Collection>> GetAllCollectionsAsync();
        Task<int> CreateCollectionAsync(CreateCollectionViewModel model, int userId);
        Task DeleteCollectionAsync(int id);
    }
}