namespace ChargeLog.Models
{
    public class DashboardMainTableRow : DashboardBase
    {
        public string? Month { get; set; }
        public string? Year { get; set; }
        public int ProviderCount { get; set; }
        public int LocationCount { get; set; }
        public double Kw { get; set; }
        public TimeSpan Duration { get; set; }

        public string? TotalAmount { get; set; }

        public string? Title { get { return $"{Month} {Year}"; } }
    }
}
