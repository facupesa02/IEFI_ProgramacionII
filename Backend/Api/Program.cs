using IEFI.Business.Interfaces;
using IEFI.Business.Services;
using IEFI.Data.Context;
using IEFI.Data.Repositories.Implementations;
using IEFI.Data.Repositories.Interfaces;
using IEFI.Business.Services;
using IEFI.Business.Interfaces;
using IEFI.Data.Repositories.Implementations;
using IEFI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
builder.Services.AddScoped<IMascotaService, MascotaService>();
builder.Services.AddScoped<IEstadiaRepository, EstadiaRepository>();
builder.Services.AddScoped<IEstadiaService, EstadiaService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
