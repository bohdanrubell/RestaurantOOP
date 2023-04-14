using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RestaurantAppOOP.models;

namespace RestaurantAppOOP.db;

public partial class RestaurantContext : DbContext
{
    public RestaurantContext()
    {
        Database.EnsureCreated();
    }

    public RestaurantContext(DbContextOptions<RestaurantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderedDish> OrderedDishes { get; set; }

    public virtual DbSet<Waiter> Waiters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(
            @"Server=(LocalDB)\MSSQLLocalDB;Database=RestaurantDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3213E83F23E41A07");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasMaxLength(65)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3213E83F8FBF43D6");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOrder)
                .HasColumnType("date")
                .HasColumnName("dateOrder");
            entity.Property(e => e.IdWaiter).HasColumnName("idWaiter");
            entity.Property(e => e.NumberOfTable).HasColumnName("numberOfTable");

            entity.HasOne(d => d.IdWaiterNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdWaiter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Waiter");
        });

        modelBuilder.Entity<OrderedDish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderedD__3213E83F5BBA48BA");

            entity.ToTable("OrderedDish");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            entity.Property(e => e.Number).HasColumnName("number");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.OrderedDishes)
                .HasForeignKey(d => d.IdMenu)
                .HasConstraintName("FK_Menu");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderedDishes)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("FK_Order");
        });

        modelBuilder.Entity<Waiter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Waiters__3213E83F495371ED");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NameWaiter)
                .HasMaxLength(50)
                .HasColumnName("nameWaiter");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
