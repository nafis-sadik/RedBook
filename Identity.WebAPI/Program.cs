using Identity.Data;
using Identity.Data.Models;
using Identity.WebAPI.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RedBook.Core.Constants;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDatabaseConfigurations(opts =>
//{
//    opts.UseSqlServer(
//        builder.Configuration["ConnectionStrings:IdentityLocal"]
//    );
//    //opts.UseMySQL(
//    //    builder.Configuration["ConnectionStrings:Identity"], sqlOpts => sqlOpts.MigrationsHistoryTable("__EFMigrationsHistory", UserManagementSystemContext.DefaultSchema).UseRelationalNulls()
//    //);
//});

builder.Services.RosolveDependencies(opts =>
{
    opts.UseSqlServer(
        builder.Configuration["ConnectionStrings:IdentityLocal"]
    );
    //opts.UseMySQL(
    //    builder.Configuration["ConnectionStrings:Identity"], sqlOpts => sqlOpts.MigrationsHistoryTable("__EFMigrationsHistory", UserManagementSystemContext.DefaultSchema).UseRelationalNulls()
    //);
});

builder.Services.AddControllers();

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

builder.Services.AddSwaggerGen(x => {
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In=ParameterLocation.Header,
        Description="Please enter token",
        Name="Authorization",
        Type=SecuritySchemeType.Http,
        BearerFormat="JWT",
        Scheme="bearer"
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
