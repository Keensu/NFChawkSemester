namespace NFChawk.Models.Entities
{
    public class Collection
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string? BannerImageUrl { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public List<NFT> NFTs { get; set; } = new();
    }
}