using ChargeLog.Models;
using Microsoft.AspNetCore.Http;

namespace ChargeLog.DBModels
{
    public class Network
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? Name { get; set; }
        public decimal Rate { get; set; }
        public bool HaveAccount { get; set; }  
        public bool IsPartner { get; set; }
        public ChargeType DefaultChargeType { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();
        public int? ImportId { get; set; }

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
