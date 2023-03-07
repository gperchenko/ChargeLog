namespace ChargeLog.DBModels
{
    public class Group
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? Name { get; set; }
        public List<Session> Sessions { get; set; }
    }
}
