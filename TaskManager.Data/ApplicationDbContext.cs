using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Data {

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid> {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Job> Tasks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
