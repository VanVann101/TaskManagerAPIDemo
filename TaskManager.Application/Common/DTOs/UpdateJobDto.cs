using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Application.Common.DTO {
    public class UpdateJobDto {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public List<Guid> UsersMemberId { get; set; }
    }
}
