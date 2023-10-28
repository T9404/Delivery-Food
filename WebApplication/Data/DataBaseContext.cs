﻿using Microsoft.EntityFrameworkCore;
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
        
        builder.Entity<Dish>().ToTable("dishes");
        builder.Entity<AddressBeforeHouse>().ToTable("AddressBeforeHouses");
        builder.Entity<AddressHouse>().ToTable("AddressAfterHouse");
        builder.Entity<HierarchyAddress>().ToTable("HierarchyAddresses");
        
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
}