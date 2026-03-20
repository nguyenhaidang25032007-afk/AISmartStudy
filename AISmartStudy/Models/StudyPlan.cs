namespace AISmartStudy.Models
{
    public class StudyPlan
    {
        public int Id { get; set; }
        
        public string Topic { get; set; } = string.Empty; // Required field
        
        public string? ContentJson { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}