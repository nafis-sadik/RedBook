using Identity.Data;
using Identity.WebAPI.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RedBook.Core.Constants;
using System.Reflection;
using System.Text;
using Yarkool.SwaggerUI;

namespace Identity.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Cross-Origin Requests (CORS)
            builder.Services.AddCorsIdentity(builder.Configuration);

            // Database Configuration
            builder.Services.AddDatabaseConfigurations(builder.Configuration);

            // IoC Container & DbContext
            builder.Services.RosolveDependencies();

            // Add Controllers
            builder.Services.AddControllers();

            // JWT Bearer token based authentication
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(CommonConstants.PasswordConfig.Salt)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Swagger Configurations
            builder.Services.AddSwaggerGen(x => {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Format: Bearer {access_token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme{
                            Reference=new OpenApiReference{
                                Id="Bearer", //The name of the previously defined security scheme.
                                Type=ReferenceType.SecurityScheme
                            }
                        },
                        new string[] { }
                    }
                });

                // Configure Swagger to use the XML file that's generated with the preceding instructions.
                // For Linux or non-Windows operating systems, file names and paths can be case-sensitive.
                // For example, a TodoApi.XML file is valid on Windows but not CentOS.
                x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            var app = builder.Build();

            // Database Initialization
            app.InitDatabase(builder.Environment);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(DefaultCorsConfig.Policy);

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            //app.UseRouting().UseAuthorization().UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //    endpoints.MapSwagger();
            //});

            //app.UseYarkoolSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/v1/swagger.json", "V1 Docs");
            //});

            app.Run();
        }
    }
}
