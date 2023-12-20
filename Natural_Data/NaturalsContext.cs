using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
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
        public virtual DbSet<DistributorToExecutive> DistributorToExecutives { get; set; }
        public virtual DbSet<Dsr> Dsrs { get; set; }
        public virtual DbSet<Dsrdetail> Dsrdetails { get; set; }
        public virtual DbSet<Executive> Executives { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Retailor> Retailors { get; set; }
        public virtual DbSet<RetailorToDistributor> RetailorToDistributors { get; set; }
        public virtual DbSet<State> States { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps<Distributor>();
            SetTimestamps<Executive>();
            SetTimestamps<Retailor>();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetTimestamps<T>() where T : class
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is T && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                }

                entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
            }
        }
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

                entity.Property(e => e.CategoryName).HasMaxLength(20);
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

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.UserName).HasMaxLength(50);

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

            modelBuilder.Entity<DistributorToExecutive>(entity =>
            {
                entity.ToTable("DistributorToExecutive");

                entity.HasIndex(e => e.ExecutiveId, "DistributortoExecutive_ibfk_1");

                entity.HasIndex(e => e.DistributorId, "DistributortoExecutive_ibfk_2");

                entity.Property(e => e.Id).HasMaxLength(10);

                entity.Property(e => e.DistributorId)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ExecutiveId)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Distributor)
                    .WithMany(p => p.DistributorToExecutives)
                    .HasForeignKey(d => d.DistributorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DistributortoExecutive_ibfk_2");

                entity.HasOne(d => d.Executive)
                    .WithMany(p => p.DistributorToExecutives)
                    .HasForeignKey(d => d.ExecutiveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DistributortoExecutive_ibfk_1");
            });

            modelBuilder.Entity<Dsr>(entity =>
            {
                entity.ToTable("DSR");

                entity.HasIndex(e => e.Distributor, "Distributor");

                entity.HasIndex(e => e.Executive, "Executive");

                entity.HasIndex(e => e.Retailor, "Retailor");

                entity.Property(e => e.Id).HasMaxLength(10);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Distributor)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Executive)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.OrderBy)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Retailor)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");

                entity.HasOne(d => d.DistributorNavigation)
                    .WithMany(p => p.Dsrs)
                    .HasForeignKey(d => d.Distributor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DSR_ibfk_2");

                entity.HasOne(d => d.ExecutiveNavigation)
                    .WithMany(p => p.Dsrs)
                    .HasForeignKey(d => d.Executive)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DSR_ibfk_1");

                entity.HasOne(d => d.RetailorNavigation)
                    .WithMany(p => p.Dsrs)
                    .HasForeignKey(d => d.Retailor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DSR_ibfk_3");
            });

            modelBuilder.Entity<Dsrdetail>(entity =>
            {
                entity.ToTable("DSRDetails");

                entity.HasIndex(e => e.Dsr, "Dsr");

                entity.HasIndex(e => e.Product, "Product");

                entity.Property(e => e.Id).HasMaxLength(10);

                entity.Property(e => e.Dsr)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.DsrNavigation)
                    .WithMany(p => p.Dsrdetails)
                    .HasForeignKey(d => d.Dsr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DSRDetails_ibfk_2");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.Dsrdetails)
                    .HasForeignKey(d => d.Product)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DSRDetails_ibfk_1");
            });

            modelBuilder.Entity<Executive>(entity =>
            {
                entity.ToTable("Executive");

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
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.UserName)
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

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.Category, "Category");

                entity.Property(e => e.Id).HasMaxLength(10);

                entity.Property(e => e.Amount)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Grams)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Products_ibfk_1");
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

            modelBuilder.Entity<RetailorToDistributor>(entity =>
            {
                entity.ToTable("RetailorToDistributor");

                entity.HasIndex(e => e.DistributorId, "FK_Distributor");

                entity.HasIndex(e => e.RetailorId, "FK_Retailor");

                entity.Property(e => e.Id).HasMaxLength(10);

                entity.Property(e => e.DistributorId).HasMaxLength(10);

                entity.Property(e => e.RetailorId).HasMaxLength(10);

                entity.HasOne(d => d.Distributor)
                    .WithMany(p => p.RetailorToDistributors)
                    .HasForeignKey(d => d.DistributorId)
                    .HasConstraintName("FK_Distributor");

                entity.HasOne(d => d.Retailor)
                    .WithMany(p => p.RetailorToDistributors)
                    .HasForeignKey(d => d.RetailorId)
                    .HasConstraintName("FK_Retailor");
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
