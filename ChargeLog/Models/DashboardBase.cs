namespace ChargeLog.Models
{
    public class DashboardBase
    {
        public int SessionCount { get; set; }
        public bool DisplayDetails { get; set; } = false;
        public decimal KWh { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
