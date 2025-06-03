using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using RentACarRWA.Data;
using RentACarRWA.Models;

namespace RentACarRWA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=users.db"));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });


            var app = builder.Build();
            var provider = new FileExtensionContentTypeProvider(); //custom MIME file provider
            provider.Mappings[".avif"] = "image/avif"; //fokusira se na ovaj

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseStaticFiles(new StaticFileOptions
                {
                    ContentTypeProvider = provider // stavlja osobinu provider od ranije na property ili atribut CTP koji je vezan za staticfileoptions
                });
                app.UseRouting();
                app.UseCors("AllowAll");
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();


                context.Cars.AddRange(
                    new Car { ModelName = "BMW IX", PricePerDay = 120, Description = "Elektricni SUV" },
                    new Car { ModelName = "Ford Mustang", PricePerDay = 95, Description = "Muscle car" },
                    new Car { ModelName = "Tesla Model S", PricePerDay = 150, Description = "Luksuzni EV" },
                    new Car { ModelName = "Audi A4", PricePerDay = 85, Description = "Komforna limuzina" },
                    new Car { ModelName = "VW Golf 8", PricePerDay = 60, Description = "Gradski auto" },
                    new Car { ModelName = "Chevrolet Bolt", PricePerDay = 29, Description = "Elektricni kompaktni automobil" },
                    new Car { ModelName = "Kia EV9", PricePerDay = 45, Description = "Elektricni SUV" },
                    new Car { ModelName = "Porsche Taycan", PricePerDay = 200, Description = "Elektricni sportski sedan" },
                    new Car { ModelName = "Audi e-Tron GT", PricePerDay = 190, Description = "Elektricni luksuzni sedan" },
                    new Car { ModelName = "Jaguar I-PACE", PricePerDay = 98, Description = "Elektricni SUV" },
                    new Car { ModelName = "Hyundai Ioniq 5", PricePerDay = 92, Description = "Elektricni crossover" },
                    new Car { ModelName = "Lexus NX Hybrid", PricePerDay = 70, Description = "Hibridni SUV" }
                );
                context.SaveChanges();


            }

            app.Run();
        }
    }
}
