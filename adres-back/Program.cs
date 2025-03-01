using adres.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
// Add services to the container.
string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

if (connectionString == null)
{
    connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=adres;";
}

builder.Services.AddDbContext<AdquisicionesContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

// Configuración de CORS
builder.Services.AddCors(options =>
{   
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://192.168.1.103", "http://192.168.1.103:4200","http://localhost", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconn", ([FromServices] AdquisicionesContext dbContext) =>
{
    dbContext.Database.Migrate();
    return Results.Ok("Database created: " + dbContext.Database.IsNpgsql());
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin"); 

app.UseAuthorization();

app.MapControllers();

app.Run("http://0.0.0.0:8080");
