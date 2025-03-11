using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities {
    public class UserTask {
        public UserTaskRole Role { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid TaskId { get; set; }
        public Job Task { get; set; }
    }
}
