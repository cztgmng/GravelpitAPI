namespace GravelpitAPI
{
    public class NewOrder
    {
        public required string id { get; set; }
        public required DateTime date { get; set; }
        public required string client { get; set; }
        public required string type { get; set; }
        public required float amount { get; set; }
        public int settled { get; set; }
        public DateTime? added_date { get; set; }
    }
}
