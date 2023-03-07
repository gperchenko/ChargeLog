namespace ChargeLog.DBModels
{
    public class Car
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? Year { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string DisplayName { get { return $"{Year} {Make} {Model}"; }}
    }
}
