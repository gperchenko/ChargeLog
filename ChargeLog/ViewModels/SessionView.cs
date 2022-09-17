using ChargeLog.Models;

namespace ChargeLog.ViewModels
{
    public class SessionView
    {
        
        public string Date { get; set; } = DateTime.Now.ToShortDateString();
        public string? Duration { get; set; }
        public string? Kwh { get; set; }
        public string? Price { get; set; }
        public bool IsActualPrice { get; set; } = true;
        public ChargeType ChargeType { get; set; }
        public int LocationId { get; set; }
        public int CarId { get; set; }
        public int? ThroughNetworkId { get; set; }

    }
}
