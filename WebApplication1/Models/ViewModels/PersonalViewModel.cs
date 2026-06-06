using NFChawk.Models.Entities;

namespace NFChawk.Models.ViewModels
{
    public class PersonalViewModel
    {
        public User User { get; set; }
        public List<NFT> MyNFTs { get; set; }
        public List<Collection> MyCollections { get; set; }
    }
}
