using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Natural_Core.Models;

#nullable disable

namespace Natural_Data
{
    public partial class NaturalsContext : DbContext
    {
        public NaturalsContext()
        {
        }

        public NaturalsContext(DbContextOptions<NaturalsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Distributor> Distributors { get; set; }
        public virtual DbSet<Executive> Executives { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Retailor> Retailors { get; set; }
        public virtual DbSet<State> States { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasIndex(e => e.CityId, "City_Id");

                entity.Property(e => e.Id).HasMaxLength(20);

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Area_Name");

                entity.Property(e => e.CityId)
                    .HasMaxLength(20)
                    .HasColumnName("City_Id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("Areas_ibfk_1");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(20)
                    .HasColumnName("Category_Name");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasIndex(e => e.StateId, "State_Id");

                entity.Property(e => e.Id).HasMaxLength(20);

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("City_Name");

                entity.Property(e => e.StateId)
                    .HasMaxLength(20)
                    .HasColumnName("State_Id");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("Cities_ibfk_1");
            });

            modelBuilder.Entity<Distributor>(entity =>
            {
                entity.ToTable("Distributor");

                entity.HasIndex(e => e.Area, "Area");

                entity.HasIndex(e => e.City, "City");

                entity.HasIndex(e => e.State, "State");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.AreaNavigation)
                    .WithMany(p => p.Distributors)
                    .HasForeignKey(d => d.Area)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Distributor_ibfk_1");

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.Distributors)
                    .HasForeignKey(d => d.City)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Distributor_ibfk_2");

                entity.HasOne(d => d.StateNavigation)
                    .WithMany(p => p.Distributors)
                    .HasForeignKey(d => d.State)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Distributor_ibfk_3");
            });

            modelBuilder.Entity<Executive>(entity =>
            {
                entity.ToTable("Executive");

                entity.HasIndex(e => e.Area, "Area");

                entity.HasIndex(e => e.City, "City");

                entity.HasIndex(e => e.State, "State");

                entity.Property(e => e.Id).HasMaxLength(50).ValueGeneratedOnAdd();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.AreaNavigation)
                    .WithMany(p => p.Executives)
                    .HasForeignKey(d => d.Area)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Executive_ibfk_1");

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.Executives)
                    .HasForeignKey(d => d.City)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Executive_ibfk_2");

                entity.HasOne(d => d.StateNavigation)
                    .WithMany(p => p.Executives)
                    .HasForeignKey(d => d.State)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Executive_ibfk_3");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<Retailor>(entity =>
            {
                entity.ToTable("Retailor");

                entity.HasIndex(e => e.Area, "Area");

                entity.HasIndex(e => e.City, "City");

                entity.HasIndex(e => e.State, "State");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.AreaNavigation)
                    .WithMany(p => p.Retailors)
                    .HasForeignKey(d => d.Area)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Retailor_ibfk_1");

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.Retailors)
                    .HasForeignKey(d => d.City)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Retailor_ibfk_2");

                entity.HasOne(d => d.StateNavigation)
                    .WithMany(p => p.Retailors)
                    .HasForeignKey(d => d.State)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Retailor_ibfk_3");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(20);

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("State_Name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
