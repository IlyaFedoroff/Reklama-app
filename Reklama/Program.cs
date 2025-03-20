using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddSingleton<IReklamaService, ReklamaService>();

builder.Services.AddControllers();

var app = builder.Build();



app.UseRouting();

app.MapControllers();

app.Run();
