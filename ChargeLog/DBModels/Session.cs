using ChargeLog.Models;

namespace ChargeLog.DBModels
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal KWh { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public ChargeType ChargeType { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public int? ThroughNetworkId { get; set; }
        public Network? ThroughNetwork { get; set; }
        public List<Group>? Groups { get; set; }
        public int? ImportId { get; set; }
    }
}
