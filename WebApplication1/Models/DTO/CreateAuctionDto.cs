using System.ComponentModel.DataAnnotations;

namespace NFChawk.Models.DTO
{
    public class CreateAuctionDto
    {
        [Required]
        public int NFTId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Start price should be more than 0")]
        public decimal StartPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Minimal step should be more than 0")]
        public decimal MinBidStep { get; set; }

        [Required]
        // На стороне контроллера добавьте проверку: EndTime > DateTime.UtcNow
        public DateTime EndTime { get; set; }
    }
}
