using CreatePgTestDatabase.Entities;

namespace CreatePgTestDatabase.Initialization;

public class IdsTestingContext : DbContext
{
    public IdsTestingContext(DbContextOptions<IdsTestingContext> options)
        : base(options)
    {
    }

    public DbSet<AllTypes>? AllTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllTypes>().HasData(AllTypesData.Copy());

        base.OnModelCreating(modelBuilder);
    }
}