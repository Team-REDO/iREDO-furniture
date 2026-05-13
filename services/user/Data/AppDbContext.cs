using Microsoft.EntityFrameworkCore;
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
        public DbSet<SavedList> SavedLists { get; set; }
        public DbSet<SavedListPost> SavedListPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Person (root)
            // =========================
            modelBuilder.Entity<Person>()
            .HasOne(person => person.Details)
            .WithOne(details => details.Person)
            .HasForeignKey<PersonDetails>(details => details.PersonId)
            .OnDelete(DeleteBehavior.Cascade);



            // Unique email (important for OAuth)
            modelBuilder.Entity<PersonDetails>()
                .HasIndex(personDetails => personDetails.Email)
                .IsUnique();

            // =========================
            // PersonDetails ↔ Address (1:1)
            // (NO navigation on Address side)
            // =========================
            modelBuilder.Entity<PersonDetails>()
                .HasOne(personDetails => personDetails.Address)
                .WithOne(address => address.PersonDetails)
                .HasForeignKey<Address>(address => address.PersonDetailsId);
            

            // =========================
            // Person ↔ SavedList (1:N)
            // =========================
            modelBuilder.Entity<SavedList>()
                .HasOne(savedList => savedList.Person)
                .WithMany(person => person.SavedLists)
                .HasForeignKey(savedList => savedList.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // SavedList ↔ SavedListPost (1:N) savedListPost
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
        }
    }
}