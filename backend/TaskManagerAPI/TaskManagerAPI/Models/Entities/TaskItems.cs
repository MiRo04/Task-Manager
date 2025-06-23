using System.Xml;

namespace TaskManagerAPI.Models.Entities
{
    public class TaskItems
    {
        public int Id { get; set; }
        public required string Title { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public required string UserId { get; set; }
        public Users User { get; set; } = null!;
    }
}
