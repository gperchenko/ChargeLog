namespace ChargeLog.Models
{
    public class LocationListItem : DashboardBase
    {
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? LocationAddress { get; set; }
    }
}
