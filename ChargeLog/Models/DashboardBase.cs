namespace ChargeLog.Models
{
    public class DashboardBase
    {
        public int SessionCount { get; set; }
        public bool DisplayDetails { get; set; } = false;
        public double KWh { get; set; }
        public TimeSpan Duration { get; set; }
        public Double Price { get; set; }
        public Double Discount { get; set; }
    }
}
