using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Data.Configurations {
    public class JobConfig : IEntityTypeConfiguration<Job> {
        public void Configure(EntityTypeBuilder<Job> builder) {
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
