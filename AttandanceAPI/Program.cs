using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using AttendanceAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Konfigurasi Serilog lengkap
builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    loggerConfig
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Semua log Microsoft, minimal Warning
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information) // Tapi ini tetap ditampilkan
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning) // Khusus hosting
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}")
        .WriteTo.File(@"D:\Logs\StudentAPI\log-.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.File(@"D:\Logs\StudentAPI\error-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
        .WriteTo.File(new JsonFormatter(), @"D:\Logs\StudentAPI\json-log-.json", rollingInterval: RollingInterval.Day);
});

// Tambahkan service
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();


builder.Services.AddDbContext<AttendanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging(); //responded log misal ok 200
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
// Jalankan aplikasi
app.Run();