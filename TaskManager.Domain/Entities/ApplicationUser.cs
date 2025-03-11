using Microsoft.AspNetCore.Identity;

namespace TaskManager.Domain.Entities {
    public class ApplicationUser : IdentityUser<Guid> {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<UserTask> UserTasks { get; set; }
    }
}