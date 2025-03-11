using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManager.Data;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManagerAPI.IntegrationTests {
    public class TaskManagerWebApplicationFactory : WebApplicationFactory<Program> {
        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            builder.ConfigureTestServices(services => {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

                services.AddInMemoryDatabase();
            });
        }
    }

    public static class ConfigureWebHostExtensions {
        public static void AddInMemoryDatabase(this IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseInMemoryDatabase("TestTaskManagerAPIDb");
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            context.Database.EnsureCreated();
            SeedDatabase(context, userManager);
        }

        private static void SeedDatabase(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
            var users = new List<ApplicationUser>
            {
            new ApplicationUser
            {
                Id = new Guid("cb563914-2d43-4dd9-bf3a-51695fa51d56"),
                UserName = "user1@example.com",
                Email = "user1@example.com",
                FirstName = "John",
                LastName = "Doe",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new ApplicationUser
            {
                Id = new Guid("12363d05-ceae-4fcf-871f-bd22476fb03d"),
                UserName = "user2@example.com",
                Email = "user2@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        };

            foreach (var user in users) {
                var result = userManager.CreateAsync(user, "Password123!").GetAwaiter().GetResult();
                if (!result.Succeeded) {
                    throw new Exception("Failed to create test user: " + string.Join(", ", result.Errors));
                }
            }

            var jobs = new List<Job>
            {
            new Job
            {
                Id = new Guid("10461ac3-c991-4a83-84a2-172648cb11a0"),
                Name = "Task 1",
                Description = "Description for Task 1",
                TaskStatus = TaskManager.Domain.Enums.TaskStatus.ToDo,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Job
            {
                Id = new Guid("cc7dc422-e7c8-4f9d-a05b-d6683cb7f213"),
                Name = "Task 2",
                Description = "Description for Task 2",
                TaskStatus = TaskManager.Domain.Enums.TaskStatus.InProgress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        };

            context.Tasks.AddRange(jobs);

            var userTasks = new List<UserTask>
            {
            new UserTask
            {
                Role = UserTaskRole.Creator,
                UserId = users[0].Id,
                TaskId = jobs[0].Id
            },
            new UserTask
            {
                Role = UserTaskRole.Member,
                UserId = users[1].Id,
                TaskId = jobs[1].Id
            }
        };

            context.UserTasks.AddRange(userTasks);
            context.SaveChanges();
        }
    }
}
