using System.ComponentModel.DataAnnotations;

namespace NFChawk.Models.ViewModels
{
    public class CreateNFTViewModel
    {
        [Required]
        public IFormFile ImageFile { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }

        public int? CollectionId { get; set; }
    }
}