namespace ChargeLog.Models
{
    public class NetworkListItem : DashboardBase
    {
        public int NetworkId { get; set; }
        public string? NetworkName { get; set; }
        public int LocationCount { get; set; }
    }
}
