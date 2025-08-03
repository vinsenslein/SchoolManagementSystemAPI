// Program.cs (Serilog + Semua Konfigurasi)

using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using SchoolManagementSystemAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Serilog Config
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    // Log ke console
    .WriteTo.Console()
    // Log harian ke folder khusus
    .WriteTo.File(
        path: @"D:\\Logs\\StudentAPI\\log-.txt",
        rollingInterval: RollingInterval.Day
    )
    // Log error khusus ke file terpisah
    .WriteTo.File(
        path: @"D:\\Logs\\StudentAPI\\error-.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Error
    )
    // Format JSON log
    .WriteTo.File(
        formatter: new JsonFormatter(),
        path: @"D:\\Logs\\StudentAPI\\json-log-.json",
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Tambahkan layanan lainnya
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
