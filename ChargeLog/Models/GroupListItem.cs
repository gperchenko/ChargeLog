namespace ChargeLog.Models
{
    public class GroupListItem : DashboardBase
    {
        public string? Name { get; set; }
        public int NetworkCount { get; set; }
        public int LocationCount { get; set; }
    }
}
