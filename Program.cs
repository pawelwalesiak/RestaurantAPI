
using System.Reflection;
using NLog.Web;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;

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

            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();
          //  builder.Services.AddTransient<IWeatherForcastService, WeatherForcastService>();
            builder.Services.AddControllers();

            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddDbContext<RestaurantDbContext>();
            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
           // builder.Services.AddScoped<RestaurantService>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            

            //recznie utworzyc scope zeby pobrac z kontnenera depdency injection
            
            
            var app = builder.Build();
            
            
            // Configure the HTTP request pipeline.

           //budowanie scope 
           var scope = app.Services.CreateScope();
           var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();

          
            app.UseHttpsRedirection();
            //app.UseAuthorization();
            app.MapControllers();
            app.Run();
            seeder.Seed();

        }
    }
}