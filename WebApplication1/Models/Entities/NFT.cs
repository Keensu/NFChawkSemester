using NFChawk.Models.Enums;

namespace NFChawk.Models.Entities
{
    public class NFT
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public NFTStatus Status { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int? CollectionId { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? OwnerId { get; set; }
        public User? Owner { get; set; }

        public Collection? Collection { get; set; }

        public Auction? Auction { get; set; }
    }
}