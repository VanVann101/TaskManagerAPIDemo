using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Application.Common.Constants;
using TaskManager.Application.Common.DTO;
using TaskManager.Application.Services;
using TaskManager.Data;
using TaskManager.Domain.Entities;

namespace TaskManagerAPI.UnitTests.ServicesTests {
    public class JobServiceTests {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private ApplicationDbContext _dbContext;

        public JobServiceTests() {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: "TestTaskDb")
             .Options;

            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetJobByIdAsync_NonExistenId_ShouldReturnResultFalse() {
            //Arrange
            var _validatorMock = new Mock<IValidator<CreateJobDto>>();
            var _jobService = new JobService(_dbContext, _validatorMock.Object, _userManagerMock.Object);

            // Act
            var result = await _jobService.GetJobByIdAsync(new Guid("12363d05-ceae-4fcf-871f-bd22476fb03d"));

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessages.NotFound, result.ErrorMessage);
        }

        [Fact]
        public async Task CreateJobsAsync_NonExistenCreatorUserId_ShouldReturnResultFalse() {
            //Arrange
            var _validatorMock = new Mock<IValidator<CreateJobDto>>();
            var _jobService = new JobService(_dbContext, _validatorMock.Object, _userManagerMock.Object);

            var jobDto = new CreateJobDto {
                Name = "New Job",
                Description = "Test Job",
                CreatorUserId = Guid.NewGuid()
            };

            _validatorMock.Setup(v => v.ValidateAsync(jobDto, default))
                          .ReturnsAsync(new ValidationResult());

            _userManagerMock.Setup(um => um.FindByIdAsync(jobDto.CreatorUserId.ToString()))
                            .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _jobService.CreateJobAsync(jobDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessages.NotFound, result.ErrorMessage);
        }
    }
}
