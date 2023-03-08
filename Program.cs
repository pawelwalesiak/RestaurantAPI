
using System.Reflection;
using NLog.Web;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;
using RestaurantAPI.Middleware;
using Microsoft.Identity.Client;
using static RestaurantAPI.Services.AccountService;
using Microsoft.AspNetCore.Identity;

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

            builder.Services.AddControllers();

            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddDbContext<RestaurantDbContext>();
            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<RequestTimeMiddleware>();
            builder.Services.AddSwaggerGen();

            


            //recznie utworzyc scope zeby pobrac z kontnenera depdency injection
            
            
            var app = builder.Build();

            
            // Configure the HTTP request pipeline.

           //budowanie scope 
           var scope = app.Services.CreateScope();
           var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
           seeder.Seed();


           app.UseResponseCaching();
           app.UseStaticFiles();
           seeder.Seed();
           app.UseSwagger();
           app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI"));
           app.UseMiddleware<RequestTimeMiddleware>(); 
           app.UseHttpsRedirection();
           app.MapControllers();
           
           app.Run();
           app.UseMiddleware<ErrorHandlingMiddleware>();
          // app.UseMiddleware<RequestTimeMiddleware>();

        }
    }
}