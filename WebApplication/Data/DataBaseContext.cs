using Microsoft.EntityFrameworkCore;
using WebApplication.Entity;

namespace WebApplication.Data;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();
        
        builder.Entity<RefreshToken>()
            .HasIndex(token => token.Token)
            .IsUnique();
    }
}