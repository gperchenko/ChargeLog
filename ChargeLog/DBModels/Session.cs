using ChargeLog.Models;

namespace ChargeLog.DBModels
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public double Kw { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public ChargeType ChargeType { get; set; }
        public int LocationId { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public int? ThroughNetworkId { get; set; }
        public Network? ThroughNetwork { get; set; }
        public List<Group>? Groups { get; set; }
    }
}
