using ChargeLog.Models;
using Microsoft.AspNetCore.Http;

namespace ChargeLog.DBModels
{
    public class Network
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Double Rate { get; set; }
        public bool HaveAccount { get; set; }  
        public bool IsPartner { get; set; }
        public ChargeType DefaultChargeType { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();

        public DateTime LastDate
        {
            get
            {
                return Locations.Any() ?
                    Locations.OrderByDescending(l => l.LastDate).First().LastDate : DateTime.MinValue;
            }
        }
    }
}
