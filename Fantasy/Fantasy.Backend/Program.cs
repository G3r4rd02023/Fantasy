using CloudinaryDotNet;
using Fantasy.Backend.Data;
using Fantasy.Backend.Helpers;
using Fantasy.Backend.Repositories.Implementations;
using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.Backend.UnitsOfWork.Implementations;
using Fantasy.Backend.UnitsOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Fantasy.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));
            builder.Services.AddTransient<SeedDb>();
            builder.Services.AddScoped<IFilesStorage, FilesStorage>();

            builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
            builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();

            builder.Services.AddScoped<ITeamsRepository, TeamsRepository>();
            builder.Services.AddScoped<ITeamsUnitOfWork, TeamsUnitOfWork>();

            var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");

            var cloudinary = new Cloudinary(new Account(
               cloudinaryConfig["CloudName"],
               cloudinaryConfig["ApiKey"],
               cloudinaryConfig["ApiSecret"]
               ));

            builder.Services.AddSingleton(cloudinary);

            var app = builder.Build();
            SeedData(app);

            void SeedData(WebApplication app)
            {
                var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
                using var scope = scopedFactory!.CreateScope();
                var service = scope.ServiceProvider.GetService<SeedDb>();
                service!.SeedAsync().Wait();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());

            app.Run();
        }
    }
}