using DOMAIN.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = DOMAIN.Models.Task;

namespace INFRASTRUCTURE.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    :DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppUser).Assembly);
    }
}