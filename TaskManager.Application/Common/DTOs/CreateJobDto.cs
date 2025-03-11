namespace TaskManager.Application.Common.DTO {
    public class CreateJobDto {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatorUserId { get; set; }
        public Guid? AssignedUserId { get; set; }
    }
}