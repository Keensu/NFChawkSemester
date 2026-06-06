using System.ComponentModel.DataAnnotations;

namespace NFChawk.Models.ViewModels
{
    public class CreateCollectionViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile BannerImage { get; set; }
    }
}
