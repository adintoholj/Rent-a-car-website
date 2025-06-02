using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using RentACarRWA.Data;

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

            app.Run();
        }
    }
}
