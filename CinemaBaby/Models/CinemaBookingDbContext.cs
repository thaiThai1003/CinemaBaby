using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CinemaBaby.Models;

public partial class CinemaBookingDbContext : DbContext
{
    public CinemaBookingDbContext()
    {
    }

    public CinemaBookingDbContext(DbContextOptions<CinemaBookingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Showtime> Showtimes { get; set; }

    public DbSet<Promotion> Promotions { get; set; }

    public DbSet<Cinema> Cinemas { get; set; }

    // ❌ ĐÃ XOÁ OnConfiguring (SQL Server)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId);

            entity.HasIndex(e => e.Username).IsUnique();

            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId);

            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); // 🔥 SQLite

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Seat).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.SeatId);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.HasIndex(e => e.Username).IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Points).HasDefaultValue(0);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId);

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("ImageURL");

            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId);

            entity.Property(e => e.IsBooked).HasDefaultValue(false);

            entity.Property(e => e.SeatNumber)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Showtime).WithMany(p => p.Seats)
                .HasForeignKey(d => d.ShowtimeId);
        });

        modelBuilder.Entity<Showtime>(entity =>
        {
            entity.HasKey(e => e.ShowtimeId);

            entity.Property(e => e.Price)
                .HasColumnType("decimal(18,2)"); // 🔥 fix nhẹ cho SQLite

            entity.Property(e => e.ShowDate);

            entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.MovieId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}