using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CinemaManager_X.Models.Cinema;

public partial class CinemaDbContext : DbContext
{
    public CinemaDbContext()
    {
    }

    public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:CinemaCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Movie__3214EC074B78E1D5");

            entity.ToTable("Movie");

            entity.Property(e => e.Genre).HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(30);

            entity.HasOne(d => d.Producer).WithMany(p => p.Movies)
                .HasForeignKey(d => d.ProducerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movie_Prod");
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producer__3214EC074265ECBA");

            entity.ToTable("Producer");

            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Nationalty).HasMaxLength(30);
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Table__3214EC07E9C12E44");

            entity.ToTable("Table");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
