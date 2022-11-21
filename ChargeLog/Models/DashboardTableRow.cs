namespace ChargeLog.Models
{
    public class DashboardMainTableRow : DashboardBase
    {
        public DateTime Date { get; set; }       
        public int NetworkCount { get; set; }
        public int LocationCount { get; set; }
        
        public string? Title { get { return $"{Date.ToString("MMMM")} {Date.ToString("yyyy")}"; } }
    }
}
