using Microsoft.EntityFrameworkCore;
using WebApplication.Entities;

namespace WebApplication.Data;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Dish> Dishes { get; set; } = null!;
    public DbSet<Basket> Baskets { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<HierarchyAddress> HierarchyAddresses { get; set; } = null!;
    public DbSet<AddressBeforeHouse> AddressBeforeHouses { get; set; } = null!;
    public DbSet<AddressHouse> AddressAfterHouse { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Dish>().ToTable("dishes", t => t.ExcludeFromMigrations());
        builder.Entity<AddressBeforeHouse>().ToTable("AddressBeforeHouses", t => t.ExcludeFromMigrations());
        builder.Entity<AddressHouse>().ToTable("AddressAfterHouse", t => t.ExcludeFromMigrations());
        builder.Entity<HierarchyAddress>().ToTable("HierarchyAddresses", t => t.ExcludeFromMigrations());
        
        builder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();
        
        builder.Entity<RefreshToken>()
            .HasIndex(token => token.Token)
            .IsUnique();
        
        builder.Entity<Basket>()
            .HasIndex(basket => basket.UserEmail)
            .IsUnique();

        builder.Entity<Order>()
            .HasIndex(order => order.UserEmail)
            .IsUnique();
    }
    
    public void InitializeDatabase()
    {
        Database.EnsureCreated();
    }

    public void MigrateDatabase()
    {
        Database.Migrate();
    }

    public void EnsureDatabaseIsCreatedAndMigrated()
    {
        if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        {
            Database.EnsureCreated();
            Database.Migrate();
        }
    }
}