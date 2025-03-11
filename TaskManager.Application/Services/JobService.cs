using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Constants;
using TaskManager.Application.Common.DTO;
using TaskManager.Application.Contracts;
using TaskManager.Data;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services {
    public class JobService : IJobService {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateJobDto> _validator;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobService(ApplicationDbContext context, IValidator<CreateJobDto> validator, UserManager<ApplicationUser> userManager) { 
            _context = context;
            _validator = validator;
            _userManager = userManager;
        }

        public async Task<Result> AssignJobToUserAsync(Guid jobId, Guid userId) {
            var job = await _context.Tasks.FindAsync(jobId);
            var user = await _context.Users.FindAsync(userId);

            if (job is null || user is null) {
                return Result.Failure(ErrorMessages.NotFound);
            }

            var ut = _context.UserTasks.FirstOrDefault(x => x.UserId == user.Id && x.TaskId == jobId);

            if (ut is null) {
                ut = new UserTask() {
                    UserId = user.Id,
                    TaskId = jobId,
                    Role = Domain.Enums.UserTaskRole.Assigned
                };

                _context.UserTasks.Add(ut);
            }
            else {
                ut.Role = Domain.Enums.UserTaskRole.Assigned;
                _context.UserTasks.Update(ut);
            }

            await _context.SaveChangesAsync();
            return Result.Success(); ;
        }

        public async Task<Result> ChangeJobStatusAsync(Guid jobId, Domain.Enums.TaskStatus status) {
            var job = await _context.Tasks.FindAsync(jobId);

            if (job is null) {
                return Result.Failure(ErrorMessages.NotFound);
            }

            job.TaskStatus = status;
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<JobDto>> CreateJobAsync(CreateJobDto jobDto) {
            var validationResult = await _validator.ValidateAsync(jobDto);
            if (!validationResult.IsValid) {
                return Result<JobDto>.Failure(validationResult.Errors.First().ErrorMessage);
            }

            if (jobDto.CreatorUserId == Guid.Empty || (await _userManager.FindByIdAsync(jobDto.CreatorUserId.ToString())) is null) {
                return Result<JobDto>.Failure(ErrorMessages.NotFound);
            }

            var job = new Job {
                Name = jobDto.Name,
                Description = jobDto.Description,
                TaskStatus = Domain.Enums.TaskStatus.ToDo,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            job.UserTasks.Add(new UserTask() {
                TaskId = job.Id,
                UserId = jobDto.CreatorUserId,
                Role = Domain.Enums.UserTaskRole.Creator
            });

            if (jobDto.AssignedUserId != null && jobDto.AssignedUserId != Guid.Empty && (await _userManager.FindByIdAsync(jobDto.AssignedUserId.Value.ToString())) != null) {
                job.UserTasks.Add(new UserTask() {
                    TaskId = job.Id,
                    UserId = jobDto.AssignedUserId.Value,
                    Role = Domain.Enums.UserTaskRole.Assigned
                });
            }

            _context.Tasks.Add(job);
            await _context.SaveChangesAsync();
            return Result<JobDto>.Success(new JobDto(job));
        }

        public async Task<Result> DeleteJobAsync(Guid jobId) {
            var job = await _context.Tasks.FindAsync(jobId);
            
            if (job is null) return Result.Failure(ErrorMessages.NotFound);

            job.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<IEnumerable<JobDto>>> GetAllJobsAsync() {
            return Result<IEnumerable<JobDto>>.Success(await _context.Tasks
            .Select(j => new JobDto(j))
            .ToListAsync());
        }

        public async Task<Result<JobDto?>> GetJobByIdAsync(Guid jobId) {
            var job = await _context.Tasks.FindAsync(jobId);
            return job != null ? Result<JobDto?>.Success(new JobDto(job)) : Result<JobDto?>.Failure(ErrorMessages.NotFound);
        }

        public async Task<Result<IEnumerable<JobDto>>> GetJobsByUserIdAsync(Guid userId) {
            //return await _context.Tasks
            //.Where(j => j.UserTasks.Conta == userId)
            //.Select(j => new JobDto(j))
            //.ToListAsync();

            throw new NotImplementedException();
        }

        public async Task<Result> UpdateJobAsync(Guid jobId, UpdateJobDto jobDto) {
            throw new NotImplementedException();
        }
    }
}