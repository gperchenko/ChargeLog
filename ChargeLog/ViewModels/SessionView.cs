using ChargeLog.Models;

namespace ChargeLog.ViewModels
{
    public class SessionView
    {
        
        public string Date { get; set; } = DateTime.Now.ToShortDateString();
        public string? Duration { get; set; }
        public string? KWh { get; set; }
        public string? Price { get; set; }
        public string? Discount { get; set; }
        public ChargeType ChargeType { get; set; }
        public int LocationId { get; set; }
        public int CarId { get; set; }
        public int? ThroughNetworkId { get; set; }

    }
}
