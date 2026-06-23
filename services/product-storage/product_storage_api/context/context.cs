using Microsoft.EntityFrameworkCore;
using models;

namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // CORE TABLES
        public DbSet<Sales_Posts> SalesPosts => Set<Sales_Posts>();
        public DbSet<SalesPost> SalesPostDetails => Set<SalesPost>();

        // EVENTS / STATE TABLES
        public DbSet<Sales_Post_Removed> SalesPostRemoved => Set<Sales_Post_Removed>();

        // LOOKUPS
        public DbSet<Color> Colors => Set<Color>();
        public DbSet<Subcategory> Subcategories => Set<Subcategory>();
        public DbSet<Image> Images => Set<Image>();

        // JOIN TABLES
        public DbSet<Sales_post_colors> SalesPostColors => Set<Sales_post_colors>();
        public DbSet<Sales_post_subcategoryId> SalesPostSubcategories => Set<Sales_post_subcategoryId>();
        public DbSet<Sales_post_images> SalesPostImages => Set<Sales_post_images>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // SALES POSTS (CORE)
            // =========================
            modelBuilder.Entity<Sales_Posts>(entity =>
            {
                entity.ToTable("Sales_Posts");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.SalesPostGuid)
                      .IsRequired();

                entity.Property(x => x.PersonGuid)
                      .IsRequired();

                entity.Property(x => x.createdAt)
                      .IsRequired();
            });

            // =========================
            // SALES POST REMOVED (EVENT TABLE)
            // =========================
            modelBuilder.Entity<Sales_Post_Removed>(entity =>
            {
                entity.ToTable("Sales_Post_Removed");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.SalesPostId)
                      .IsRequired();

                entity.Property(x => x.removedAt)
                      .IsRequired();
            });

            // =========================
            // ENUM mapping (important)
            // =========================
            modelBuilder.Entity<SalesPost>(entity =>
            {
                entity.ToTable("SalesPostDetails");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Condition)
                      .HasConversion<int>(); // stores enum as int in SQL Server
            });

            // =========================
            // JOIN TABLES
            // =========================

            modelBuilder.Entity<Sales_post_colors>(entity =>
            {
                entity.ToTable("SalesPostColors");
                entity.HasKey(x => new { x.SalesPostId, x.ColorId });
            });

            modelBuilder.Entity<Sales_post_subcategoryId>(entity =>
            {
                entity.ToTable("SalesPostSubcategories");
                entity.HasKey(x => new { x.SalesPostId, x.subcategoryId });
            });

            modelBuilder.Entity<Sales_post_images>(entity =>
            {
                entity.ToTable("SalesPostImages");
                entity.HasKey(x => new { x.SalesPostId, x.imageId });
            });
        }
    }
}