using ChargeLog.DBModels;

namespace ChargeLog.Models
{
    public class SessionListItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal KWh { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string? ChargeType { get; set; }       
        public string? Car { get; set; }
        public string? ThroughNetwork { get; set; }
    }
}
