using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;

namespace RestaurantAPI.Entities
{
    public class RestaurantDbContext : DbContext
    {
        //   private string _connectionString = "Data Source=.\\;Initial Catalog=RestaurantDb;Integrated Security=True;Trusted_Connection=True;Encrypt=false";
        //Data Source=.\;Initial Catalog=RestaurantDb;Integrated Security=True
        //kontekst bazy danych 
        //odwzorwoanie klas na tabele 
        //publiczne wlasnicwosci ktore jak ogeneryczny parametr przyjmuja typk ktory bedzie prezentowqac dana tabele 

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            
        }

        public DbSet<Restaurant> Restaurants { get; set; } 
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }  
        public DbSet<Role> Roles { get; set; }

        // wymagane dane - dodatkowa konfiguracja nadpisana w metodzie on model creating 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(u => u.Name)
                .IsRequired();


            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(21); 
            
            modelBuilder.Entity<Dish>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(21);
            modelBuilder.Entity<Address>() 
                .Property(a=>a.City)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Address>() 
                .Property(a=>a.Street)
                .IsRequired()
                .HasMaxLength(50);
        }

      //  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      //  {
      //      optionsBuilder.UseSqlServer(_connectionString);

      // }
    }
}
