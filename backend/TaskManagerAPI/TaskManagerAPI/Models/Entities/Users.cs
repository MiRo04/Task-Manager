using Microsoft.AspNetCore.Identity;

namespace TaskManagerAPI.Models.Entities
{
    public class Users : IdentityUser
    {
        public ICollection<TaskItems> TaskItems { get; set; } = new List<TaskItems>();
    }
}
