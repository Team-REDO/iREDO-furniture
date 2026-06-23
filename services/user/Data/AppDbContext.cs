using Microsoft.EntityFrameworkCore;
using UserService.DomainModels;

namespace user.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonDetails> Person_Details { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<PersonRemoved> Person_Removed { get; set; }
        public DbSet<SavedList> Saved_Lists { get; set; }
        public DbSet<SavedListPost> Saved_List_Posts { get; set; }
        public DbSet<ProcessedEvent> Processed_Events { get; set; }
        //ProcessedEvent
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
            base.OnModelCreating(modelBuilder);

            // =========================
            // Person ↔ PersonDetails (1:N)
            // =========================
            modelBuilder.Entity<PersonDetails>()
                .HasOne(d => d.Person)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // Person ↔ Address (1:N)
            // =========================
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Person)
                .WithMany(p => p.Addresses)
                .HasForeignKey(a => a.PersonId);

            // =========================
            // Person ↔ SavedList (1:N)
            // =========================
            modelBuilder.Entity<SavedList>()
                .HasOne(sl => sl.Person)
                .WithMany(p => p.SavedLists)
                .HasForeignKey(sl => sl.PersonId);

            // =========================
            // SavedList ↔ SavedListPost (1:N)
            // =========================
            modelBuilder.Entity<SavedListPost>()
                .HasKey(x => new
                {
                    x.SavedListId,
                    x.SalesPostGuid
                });

            modelBuilder.Entity<SavedListPost>()
                .HasOne(x => x.SavedList)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.SavedListId);

            // =========================
            // Person ↔ PersonRemoved (1:N)
            // =========================
            modelBuilder.Entity<PersonRemoved>()
               .HasOne(pr => pr.Person)
               .WithMany(p => p.RemovedRecords)
               .HasForeignKey(pr => pr.PersonId);

            // =========================
            // Person ↔ PersonRemoved (1:N)
            // =========================
            modelBuilder.Entity<Person>()
               .HasOne(p => p.Role)
               .WithMany(r => r.Persons)
               .HasForeignKey(p => p.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // ProcessedEvent
            // =========================
            modelBuilder.Entity<ProcessedEvent>()
                .HasKey(x => x.EventId);

            // =========================
            // Alternate Key
            // =========================
            modelBuilder.Entity<Person>()
                .HasIndex(x => x.PersonGuid)
                .IsUnique();
            
            // =========================
            // Seeding default roles
            // =========================
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "User"
                },
                new Role
                {
                    Id = 2,
                    Name = "Admin"
                }
            );
            
        }
    }
}