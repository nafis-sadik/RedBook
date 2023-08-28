using Inventory.Data;
using Inventory.WebAPI.Configurations;
using Microsoft.OpenApi.Models;

namespace Inventory.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. 
            // Caching
            builder.Services.AddCaching(builder.Configuration);

            // Cross-Origin Requests (CORS)
            builder.Services.AddCors(builder.Configuration);

            // Database Connection Configuration
            builder.Services.AddDatabaseConfigurations(builder.Configuration);

            // IoC Container & DbContext
            builder.Services.RosolveDependencies(builder.Configuration);

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddSwaggerGen();

            // Swagger Configurations
            builder.Services.AddSwaggerGen(x => {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme{
                            Reference=new OpenApiReference{
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            var app = builder.Build();

            // Database Initialization
            app.InitDatabase(builder.Environment);

            app.UseCors();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
