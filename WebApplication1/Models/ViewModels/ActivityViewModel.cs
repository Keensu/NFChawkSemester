using NFChawk.Models.Entities;

namespace NFChawk.Models.ViewModels
{
    public class ActivityViewModel
    {
        public List<ActivityLog> Activities { get; set; } = new List<ActivityLog>();
        public string? CurrentFilter { get; set; }
    }
}
