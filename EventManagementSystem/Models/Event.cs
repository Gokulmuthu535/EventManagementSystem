namespace EventManagementSystem.Models
{
    public class Event
    {

        public int EventID { get; set; } 
        public string? Name { get; set; } 
        public DateTime? Date { get; set; } 
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public string? EventType { get; set; }
    }
}
