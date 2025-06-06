using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DOMAIN.Models.ModelConfigs;

public class RefreshTokenConfig:IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(t => t.IsActive).HasColumnName("IsActive");
    }
}