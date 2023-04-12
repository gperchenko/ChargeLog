namespace ChargeLog.Models
{
    public class ImportListItem
    {
        public DateTime CreateDate { get; set; }
        public string? Type { get; set; }
        public string? FileName { get; set; }
        public int NetworkCount { get; set; }
        public int LocationCount { get; set; }
        public int SessionCount { get; set; }
    }
}
