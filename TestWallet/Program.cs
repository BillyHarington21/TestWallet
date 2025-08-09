using Microsoft.EntityFrameworkCore;
using Serilog;
using TestWallet.API.Endpoints;
using TestWallet.Application.Interfaces;
using TestWallet.Application.Services;
using TestWallet.Domain.Interfaces;
using TestWallet.Infrastructure.Data;
using TestWallet.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Логирование
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Подключение к PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Репозитории
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Сервисы
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IBalanceManagementService, BalanceManagementService>();

// Minimal API endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapRegistrationEndpoints();
app.MapBalanceManagementEndpoints();

app.Run();
