
using System.Reflection;
using NLog.Web;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;
using RestaurantAPI.Middleware;
using Microsoft.Identity.Client;
using static RestaurantAPI.Services.AccountService;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            //utowrzenie webhosta
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //metoda configure service zalezlonsci kontenea dep 
            //rejestracja zaleznosci dependency innjeciotn w kontenerze

            var authenticationSettigns = new AuthenticationSettings();

            
            builder.Configuration.GetSection("Authentication").Bind(authenticationSettigns);
            builder.Services.AddSingleton(authenticationSettigns);
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = authenticationSettigns.JwtIssuer,
                    ValidAudience = authenticationSettigns.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettigns.JwtKey)),
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "Polish"));
                options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
                
            });

            builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();

              builder.Services.AddControllers().AddFluentValidation();
            //builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();
            



            builder.Services.AddScoped<RestaurantSeeder>();
         
            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
            builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();
            builder.Services.AddScoped<RequestTimeMiddleware>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendClient", builder => 
                         builder.AllowAnyMethod()
                                .AllowAnyMethod()
                                .WithOrigins("http://localhost:8080"));
            }
            );

            builder.Services.AddDbContext<RestaurantDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDbConnection")));



            //recznie utworzyc scope zeby pobrac z kontnenera depdency injection

            var app = builder.Build();


            //budowanie scope 
            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();

            app.UseResponseCaching();
            app.UseStaticFiles();
            app.UseCors("FrontendClient");
            

            seeder.Seed();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
          //  app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI"));
           // app.MapControllers();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }
            );

            app.Run();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            // app.UseMiddleware<RequestTimeMiddleware>();
        }
    }
}