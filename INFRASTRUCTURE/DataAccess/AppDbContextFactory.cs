using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace INFRASTRUCTURE.DataAccess;

public class AppDbContextFactory:IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // var config = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile(Path.Combine("..", "API", "appsettings.json"))
        //     .Build();
        //
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        // var connectionString = config.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer("Data Source=OZZY;Initial Catalog = Taskozz;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

        return new AppDbContext(optionsBuilder.Options);
    }
}