using TaskManager.Application.Common;
using TaskManager.Application.Common.DTO;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Application.Contracts {
    public interface IJobService {
        Task<Result<JobDto?>> GetJobByIdAsync(Guid jobId);
        Task<Result<IEnumerable<JobDto>>> GetAllJobsAsync();
        Task<Result<IEnumerable<JobDto>>> GetJobsByUserIdAsync(Guid userId);  
        Task<Result<JobDto>> CreateJobAsync(CreateJobDto jobDto);
        Task<Result> UpdateJobAsync(Guid jobId, UpdateJobDto jobDto);
        Task<Result> DeleteJobAsync(Guid jobId);  
        Task<Result> AssignJobToUserAsync(Guid jobId, Guid userId); 
        Task<Result> ChangeJobStatusAsync(Guid jobId, TaskStatus status);
    }
}
