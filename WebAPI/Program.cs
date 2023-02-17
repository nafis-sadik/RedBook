using Inventory.Domain;
using Inventory.WebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Caching
builder.Services.AddCaching(builder.Configuration);
// Cross-Origin Requests (CORS)
builder.Services.AddCors(builder.Configuration);

builder.Services.AddDatabaseConfigurations(builder.Configuration);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Database Initialization
app.InitDatabase(builder.Environment);

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
