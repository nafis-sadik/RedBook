using Identity.Data;
using Identity.WebAPI.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RedBook.Core.Constants;
using System.Reflection;
using System.Text;
using Yarkool.SwaggerUI;
using Z.SwaggerUI.Extensions;
//https://localhost:7093/swagger/index.html#/home
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
                x.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Blume Digital Corp. - Identity API",
                    Version = "v1",
                    Description = "A JWT based user management system to authenticate and manage users, their roles, rights and other user related features for all applications developed and maintained by Blume Digital Corp.",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });

                x.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction!.ControllerName + "-" + controllerAction.ActionName;
                });

                foreach (var file in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
                {
                    x.IncludeXmlComments(file);
                }

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Format: Bearer {access_token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    //BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme{
                            Reference=new OpenApiReference{
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
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
                //app.UseSwaggerUI();
            }


            app.MapControllers();

            app.UseCors(DefaultCorsConfig.Policy);

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting().UseAuthorization().UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapSwagger();
            });

            //app.UseZSwaggerUI(title: "/v1/swagger.json", templateName: "V1 Docs");

            app.UseYarkoolSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/v1/swagger.json", "V1 Docs");
            });

            app.Run();
        }
    }
}
