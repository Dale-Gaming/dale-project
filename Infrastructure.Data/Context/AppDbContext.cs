using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var method = typeof(ModelBuilder)
                .GetMethods()
                .First(m => m.Name == "Entity" && m.IsGenericMethod && m.GetParameters().Length == 0);

            var genericMethod = method.MakeGenericMethod(entityType.ClrType);
            var entityBuilder = genericMethod.Invoke(modelBuilder, null);

            var toTableMethod = entityBuilder!.GetType().GetMethod("ToTable", new[] { typeof(string) });
            toTableMethod?.Invoke(entityBuilder, new object[] { entityType.ClrType.Name });
        }

        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Nickname)
            .IsUnique();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<DaleEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
