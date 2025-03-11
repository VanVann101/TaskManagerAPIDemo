using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Data.Configurations;
public class UserTaskConfig : IEntityTypeConfiguration<UserTask> {
    public void Configure(EntityTypeBuilder<UserTask> builder) {
        builder.HasKey(x => new { x.UserId, x.TaskId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserTasks)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Task)
            .WithMany(x => x.UserTasks)
            .HasForeignKey(x => x.TaskId);
    }
}