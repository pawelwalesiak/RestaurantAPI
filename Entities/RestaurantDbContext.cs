using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;

namespace RestaurantAPI.Entities
{
    public class RestaurantDbContext : DbContext
    {
        private string _connectionString = "Data Source=.\\;Initial Catalog=RestaurantDb;Integrated Security=True;Trusted_Connection=True;Encrypt=false";
        //Data Source=.\;Initial Catalog=RestaurantDb;Integrated Security=True
        //kontekst bazy danych 
        //odwzorwoanie klas na tabele 
        //publiczne wlasnicwosci ktore jak ogeneryczny parametr przyjmuja typk ktory bedzie prezentowqac dana tabele 

        public DbSet<Restaurant> Restaurants { get; set; } 
        public DbSet<Address> Addresses { get; set; }
        public  DbSet<Dish> Dishes { get; set; }

        // wymagane dane - dodatkowa konfiguracja nadpisana w metodzie on model creating 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

        }
    }
}
