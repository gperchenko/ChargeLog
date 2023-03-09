namespace ChargeLog.DBModels
{
    public class Import
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? Type { get; set; }
        public string? FileName { get; set; }
        public int NetworkCount { get; set; }
        public int LocationCount { get; set; }
        public int SessionCount { get; set; }
    }
}
