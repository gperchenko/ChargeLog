 namespace ChargeLog.DBModels
{
    public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int NetworkId { get; set; }
        public Network? Network { get; set; }
        public List<Session> Sessions { get; set; } = new List<Session>(); 
        public DateTime LastDate
        { 
            get
            {
                return Sessions.Any() ? 
                    Sessions.OrderByDescending(s => s.Date).First().Date : DateTime.MinValue;
            }  
        }
    }
}
