using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DOMAIN.Models.ModelConfigs;
// will config on APP level

public class TaskConfig:IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(t=>t.Id);
        builder.HasOne(t => t.User)
            .WithMany(u => u.Tasks);
        
        builder.Property(t => t.Title).IsRequired().HasMaxLength(256);
        builder.Property(t => t.Description).HasMaxLength(4096);
        builder.Property(t=>t.IconUrl).IsRequired().HasMaxLength(2048);
    }
}