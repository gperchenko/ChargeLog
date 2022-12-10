namespace ChargeLog.Models
{
    public class NetworkListItem : DashboardBase
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int LocationCount { get; set; }
    }
}
