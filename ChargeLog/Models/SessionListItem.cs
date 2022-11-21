using ChargeLog.DBModels;

namespace ChargeLog.Models
{
    public class SessionListItem
    {
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public double KWh { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string? ChargeType { get; set; }       
        public string? Car { get; set; }
        public string? ThroughNetwork { get; set; }
    }
}
