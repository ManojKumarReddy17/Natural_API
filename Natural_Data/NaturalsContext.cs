using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Natural_Core.Models.CustomModels;

#nullable disable

namespace Natural_Core.Models
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
        public virtual DbSet<DistributorNotification> DistributorNotifications { get; set; }
        public virtual DbSet<DistributorToExecutive> DistributorToExecutives { get; set; }
        public virtual DbSet<DistributorbyArea> DistributorbyAreas { get; set; }
        public virtual DbSet<Dsr> Dsrs { get; set; }
        public virtual DbSet<Dsrdetail> Dsrdetails { get; set; }
        public virtual DbSet<Executive> Executives { get; set; }
        public virtual DbSet<ExecutiveArea> ExecutiveAreas { get; set; }
        public virtual DbSet<ExecutiveGp> ExecutiveGps { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationDistributor> NotificationDistributors { get; set; }
        public virtual DbSet<NotificationExecutive> NotificationExecutives { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }

        public virtual DbSet<Retailor> Retailors { get; set; }
        public virtual DbSet<RetailorToDistributor> RetailorToDistributors { get; set; }
        public virtual DbSet<State> States { get; set; }


        // **this is StoreProcedure Model we have to include Mannually **
        public virtual DbSet<DistributorSalesReport> DistributorSalesReports { get; set; }
        public virtual DbSet<DistributorReport> DistributorReports { get;set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps<Distributor>();
            SetTimestamps<Executive>();
            SetTimestamps<Retailor>();
            //SetTimestamps<Area>();
            SetTimestamps<Product>();
            //SetTimestamps<Dsr>();
            SetTimestamps<Notification>();

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
                entity.HasIndex(e => e.CityId, "Areas_ibfk_1");

                entity.Property(e => e.Id).HasMaxLength(20);

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Area_Name");

                entity.Property(e => e.CityId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("City_Id");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Areas_ibfk_1");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasIndex(e => e.StateId, "Cities_ibfk_1");

                entity.Property(e => e.Id).HasMaxLength(20);

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("City_Name");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.StateId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("State_Id");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Cities_ibfk_1");
            });

            modelBuilder.Entity<Distributor>(entity =>
            {
                entity.ToTable("Distributor");

                //entity.HasIndex(e => e.Area, "Area");

                entity.HasIndex(e => e.City, "City");

                entity.HasIndex(e => e.State, "State");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                //entity.Property(e => e.Area)
                //    .IsRequired()
                //    .HasMaxLength(20);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(50);
                entity.Property(e => e.Executive).HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Longitude).HasMaxLength(50);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.UserName).HasMaxLength(50);

                //entity.HasOne(d => d.AreaNavigation)
                //    .WithMany(p => p.Distributors)
                //    .HasForeignKey(d => d.Area)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("Distributor_ibfk_1");

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

                entity.HasIndex(e => e.DistributorId, "FK_Distributor");

                entity.HasIndex(e => e.ExecutiveId, "FK_Executive");

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
                    .HasConstraintName("DistributorToExecutive_ibfk_2");

                entity.HasOne(d => d.Executive)
                    .WithMany(p => p.DistributorToExecutives)
                    .HasForeignKey(d => d.ExecutiveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DistributorToExecutive_ibfk_1");
            });

            modelBuilder.Entity<DistributorbyArea>(entity =>
            {
                //entity.ToTable("DistributorbyArea");

                entity.HasIndex(e => e.DistributorId, "DistributorbyArea_ibfk_1");

                //entity.HasIndex(e => e.AreaId, "DistributorbyArea_ibfk_2");

                //entity.Property(e => e.AreaId)
                //    .IsRequired()
                //    .HasMaxLength(20);

                entity.Property(e => e.DistributorId)
                    .IsRequired()
                    .HasMaxLength(50);

                //entity.HasOne(d => d.Area)
                //    .WithMany(p => p.DistributorbyAreas)
                //    .HasForeignKey(d => d.AreaId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("DistributorbyArea_ibfk_2");

                //entity.HasOne(d => d.Distributor)
                //    .WithMany(p => p.DistributorbyAreas)
                //    .HasForeignKey(d => d.DistributorId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("DistributorbyArea_ibfk_1");
            });

            modelBuilder.Entity<Dsr>(entity =>
            {
                entity.ToTable("DSR");

                entity.HasIndex(e => e.Executive, "DSR_ibfk_1");

                entity.HasIndex(e => e.Distributor, "DSR_ibfk_2");

                entity.HasIndex(e => e.Retailor, "DSR_ibfk_3");

                entity.HasIndex(e => e.OrderBy, "DSR_ibfk_4");

                entity.HasIndex(e => e.Area, "DSR_ibfk_5");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Distributor)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Executive)
                    .IsRequired()
                    .HasMaxLength(50);

               

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OrderBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Retailor)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.Area)
                   .IsRequired()
                   .HasMaxLength(50);

                entity.Property(e => e.TotalAmount).HasPrecision(20, 3);

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

                entity.HasOne(d => d.OrderByNavigation)
                    .WithMany(p => p.Dsrs)
                    .HasForeignKey(d => d.OrderBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DSR_ibfk_4");

                entity.HasOne(d => d.RetailorNavigation)
                    .WithMany(p => p.Dsrs)
                    .HasForeignKey(d => d.Retailor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DSR_ibfk_3");

                entity.HasOne(d => d.AreaNavigation)
                    .WithMany(p => p.Dsrs)
                    .HasForeignKey(d => d.Area)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DSR_ibfk_5");
            });

            modelBuilder.Entity<Dsrdetail>(entity =>
            {
                entity.ToTable("DSRDetails");

                entity.HasIndex(e => e.Dsr, "DSRDetails_ibfk_2");

                entity.HasIndex(e => e.Product, "Product");

                entity.Property(e => e.Dsr)
                    .IsRequired()
                    .HasMaxLength(50);

              

                entity.Property(e => e.Price).HasPrecision(20, 3);

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(50);

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

                entity.HasIndex(e => e.City, "City");

                entity.HasIndex(e => e.State, "State");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Longitude).HasMaxLength(50);

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

            modelBuilder.Entity<ExecutiveArea>(entity =>
            {
                entity.ToTable("Executive_Area");

                entity.HasIndex(e => e.Area, "Area");

                entity.HasIndex(e => e.Executive, "Executive");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Executive)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.AreaNavigation)
                    .WithMany(p => p.ExecutiveAreas)
                    .HasForeignKey(d => d.Area)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Executive_Area_ibfk_2");

                entity.HasOne(d => d.ExecutiveNavigation)
                    .WithMany(p => p.ExecutiveAreas)
                    .HasForeignKey(d => d.Executive)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Executive_Area_ibfk_1");
            });

            modelBuilder.Entity<ExecutiveGp>(entity =>
            {
                entity.ToTable("Executive_GPS");

                entity.HasIndex(e => e.ExecutiveId, "Executive_GPS_ibfk_1");

                entity.Property(e => e.ExecutiveId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Longitude)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Executive)
                    .WithMany(p => p.ExecutiveGps)
                    .HasForeignKey(d => d.ExecutiveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Executive_GPS_ibfk_1");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("'1'");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Body).HasMaxLength(3000);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<NotificationDistributor>(entity =>
            {
                entity.ToTable("Notification_Distributor");

                entity.HasIndex(e => e.Distributor, "Notification_Distributor_ibfk_1");

                entity.HasIndex(e => e.Notification, "Notification_Distributor_ibfk_2");

                entity.Property(e => e.Distributor)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.Notification)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DistributorNavigation)
                    .WithMany(p => p.NotificationDistributors)
                    .HasForeignKey(d => d.Distributor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Notification_Distributor_ibfk_1");

                entity.HasOne(d => d.NotificationNavigation)
                    .WithMany(p => p.NotificationDistributors)
                    .HasForeignKey(d => d.Notification)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Notification_Distributor_ibfk_2");
            });

            modelBuilder.Entity<NotificationExecutive>(entity =>
            {
                entity.ToTable("Notification_Executive");

                entity.HasIndex(e => e.Notification, "Notification_Executive_ibfk_2_idx");

                entity.HasIndex(e => e.Executive, "Notification_Executive_idk");

                entity.Property(e => e.Executive)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.Notification)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ExecutiveNavigation)
                    .WithMany(p => p.NotificationExecutives)
                    .HasForeignKey(d => d.Executive)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NT_Ex");

                entity.HasOne(d => d.NotificationNavigation)
                    .WithMany(p => p.NotificationExecutives)
                    .HasForeignKey(d => d.Notification)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NT_Ex_2");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasIndex(e => e.ProductType, "FK_ProductType");



                entity.HasIndex(e => e.Category, "Product_ibfk_1");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasPrecision(20, 3);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Product_Name");
                entity.Property(e => e.ProductType).HasMaxLength(30);
                entity.Property(e => e.DisplayPrice).HasPrecision(20, 3);


                entity.Property(e => e.Weight).HasPrecision(20, 3);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Product_ibfk_1");
                entity.HasOne(d => d.ProductTypeNavigation)
                   .WithMany(p => p.Products)
                   .HasForeignKey(d => d.ProductType)
                   .HasConstraintName("FK_ProductType");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.ToTable("ProductType");

                entity.Property(e => e.Id).HasMaxLength(20);
                entity.Property(e => e.ProductTypeCode)
                   .HasMaxLength(1)
                   .IsFixedLength(true);


                entity.Property(e => e.ProductTypeName).HasMaxLength(30);
            });
            modelBuilder.Entity<Retailor>(entity =>
            {
                entity.ToTable("Retailor");


                //entity.HasIndex(e => e.Area, "Area");

                entity.HasIndex(e => e.City, "City");

                entity.HasIndex(e => e.State, "State");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                //entity.Property(e => e.Area)
                //    .IsRequired()
                //    .HasMaxLength(20);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(50);
                entity.Property(e => e.Distributor).HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Longitude).HasMaxLength(50);

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20);

                //entity.HasOne(d => d.AreaNavigation)
                //    .WithMany(p => p.Retailors)
                //    .HasForeignKey(d => d.Area)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("Retailor_ibfk_1");

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

                entity.Property(e => e.DistributorId)
                    .IsRequired()
                    .HasMaxLength(10);

               

                entity.Property(e => e.RetailorId)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Distributor)
                    .WithMany(p => p.RetailorToDistributors)
                    .HasForeignKey(d => d.DistributorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Distributor");

                entity.HasOne(d => d.Retailor)
                    .WithMany(p => p.RetailorToDistributors)
                    .HasForeignKey(d => d.RetailorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Retailor");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(20);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("State_Name");
            });
            //include Manually sp model
            modelBuilder.Entity<DistributorSalesReport>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<DistributorReport>(entity =>
            {
                entity.HasNoKey();
            });

            //modelBuilder.Entity<DistributorReport>(entity =>
            //{
            //    entity.HasNoKey();
            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
