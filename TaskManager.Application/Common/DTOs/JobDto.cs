using TaskManager.Domain.Entities;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Application.Common.DTO {
    public class JobDto {
        public JobDto(Job job) {
            Id = job.Id;
            Name = job.Name;
            Description = job.Description;
            TaskStatus = job.TaskStatus;
            CreatedAt = job.CreatedAt;
            UpdatedAt = job.UpdatedAt;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
