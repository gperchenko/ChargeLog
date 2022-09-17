using ChargeLog.Models;

namespace ChargeLog.DBModels
{
    public class Network
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Double Rate { get; set; }
        public bool HaveAccount { get; set; }     
        public ChargeType DefaultChargeType { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();
    }
}
