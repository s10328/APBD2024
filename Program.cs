//Manage NuGet - instalacja pakietow
//Microsoft.EntityFrameworkCore
// Microsoft.EntityFrameworkCore.SqlServer
// Microsoft.EntityFrameworkCore.Tools

//Struktura folderow
// Models - krok 1
// Data - krok 2
// Services
// Controllers
// DTOs

//pamietaj o konfiguracji polaczenia w pliku appsetting.json
//{
// "ConnectionStrings": {
//   "DefaultConnection": "Server=localhost;Database=PrescriptionDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True"
// },

//zeby otworzyc terminal 2x shift
//utworzenie pierwszej migracji
//dotnet ef migrations add InitialMigration - tutaj mozna zmieniac nazwe
//jesli sie uda faktycznie utworzenie bazy danych
//dotnet ef database update
//usuwanie migracji
//dotnet ef migrations remove

//uruchomienie dockera
//docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" -p 1433:1433 --name sql_server -d mcr.microsoft.com/mssql/server:2019-latest

using WebApplication2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

//krok 4 dodanie DBContext do kontenera DI
builder.Services.AddDbContext<PrescriptionDbContext>(options => //w odniesieniu do PrescritpionDBContext w folderze Data
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>(); //pierwszy kontroller
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Prescription API", 
        Version = "v1",
        Description = "API do zarządzania receptami i pacjentami"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
