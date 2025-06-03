using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DOMAIN.Models.ModelConfigs;
 // will config on APP level
public class AppUserConfig:IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u=> u.GoogleId).IsUnique();
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(128);
        builder.Property(u => u.IconUrl).IsRequired().HasMaxLength(2048);
    }
}