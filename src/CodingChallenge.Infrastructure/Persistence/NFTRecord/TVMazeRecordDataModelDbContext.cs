
using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Infrastructure.Persistence.TVMazeRecord;


public class TVMazeRecordDataModelDbContext : DbContext
{
    public TVMazeRecordDataModelDbContext(DbContextOptions<TVMazeRecordDataModelDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TVMazeRecordDataModelConfiguration());
    }

    public DbSet<TVMazeRecordDataModel> NftDataModel { get; set; }
}
