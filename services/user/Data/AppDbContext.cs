using Microsoft.EntityFrameworkCore;
using user.DomainModels;
using UserService.DomainModels;

namespace user.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonDetails> PersonDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PersonRemoved> PersonRemoved { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SavedList> SavedLists { get; set; }
        public DbSet<SavedListPost> SavedListPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Person ↔ PersonDetails (1:1)
            // =========================
            modelBuilder.Entity<Person>()
                .HasOne(person => person.Details)
                .WithOne(details => details.Person)
                .HasForeignKey<PersonDetails>(details => details.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique email
            modelBuilder.Entity<PersonDetails>()
                .HasIndex(details => details.Email)
                .IsUnique();

            // =========================
            // Person ↔ Address (1:1)
            // =========================
            modelBuilder.Entity<Person>()
                .HasOne(person => person.Address)
                .WithOne(address => address.Person)
                .HasForeignKey<Address>(address => address.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // Role ↔ Person (1:N)
            // =========================
            modelBuilder.Entity<Person>()
                .HasOne(person => person.Role)
                .WithMany(role => role.Persons)
                .HasForeignKey(person => person.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // Person ↔ SavedList (1:N)
            // =========================
            modelBuilder.Entity<SavedList>()
                .HasOne(savedList => savedList.Person)
                .WithMany(person => person.SavedLists)
                .HasForeignKey(savedList => savedList.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // SavedList ↔ SavedListPost (1:N)
            // =========================
            modelBuilder.Entity<SavedListPost>()
                .HasOne(savedListPost => savedListPost.SavedList)
                .WithMany(savedList => savedList.Posts)
                .HasForeignKey(savedListPost => savedListPost.SavedListId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // Person ↔ PersonRemoved (1:N)
            // =========================
            modelBuilder.Entity<PersonRemoved>()
                .HasOne(personRemoved => personRemoved.Person)
                .WithMany(person => person.RemovedRecords)
                .HasForeignKey(personRemoved => personRemoved.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // Seed Roles
            // =========================
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "user"
                },
                new Role
                {
                    Id = 2,
                    Name = "admin"
                }
            );
        }
    }
}