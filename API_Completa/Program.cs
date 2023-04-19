using API_Completa;
using API_Completa.Datos;
using API_Completa.Repositorio;
using API_Completa.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AplicationDbContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // Acceder a la cadena de conexion
});

// Crear el servicio de AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));


builder.Services.AddScoped<IApiRepositorio, ApiRepositorio>();
builder.Services.AddScoped<INumeroApiRepositorio, NumeroApiRepositorio>();

var app = builder.Build();

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
