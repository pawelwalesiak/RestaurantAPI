
using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //utowrzenie webhosta
            //zostaje
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            //metoda configure service zalezlonsci kontenea dep 
            //rejestracja zaleznosci dependency innjeciotn w kontenerze

            var conectionstring = builder.Configuration.GetConnectionString("Data Source=.\\;Initial Catalog=RestaurantDb;Integrated Security=True;Trusted_Connection=True;Encrypt=false");

            builder.Services.AddTransient<IWeatherForcastService, WeatherForcastService>();
            builder.Services.AddControllers();

            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddDbContext<RestaurantDbContext>();
            builder.Services.AddScoped<RestaurantSeeder>();
            //recznie utworzyc scope zeby pobrac z kontnenera depdency injection
            
            
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.

           //budowanie scope 
           var scope = app.Services.CreateScope();
           var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
           seeder.Seed();

            app.UseHttpsRedirection();
            //app.UseAuthorization();
            app.MapControllers();
            app.Run();
            

        }
    }
}