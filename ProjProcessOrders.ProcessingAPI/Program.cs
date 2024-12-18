using AspNetCore.SwaggerUI.Themes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjProcessOrders.Application.Services;
using ProjProcessOrders.Composition;
using ProjProcessOrders.Infrastructure.Context;
using ProjProcessOrders.Localization.Localizations;
using ProjProcessOrders.Messaging;
using ProjProcessOrders.UseCase.UseCases.CreateClient;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadName()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {TraceIdentifier}{NewLine}{Exception}")
                .MinimumLevel.Information()
                .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddSingleton(Log.Logger);

builder.Services.ConfigureApplicantionApp();

builder.Services.AddInfrastructureServices(builder.Configuration);

await Task.Delay(TimeSpan.FromSeconds(15));

builder.Services.AddSingleton<RabbitMqConfig>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    string hostName = configuration["RabbitMqSettings:HostName"];
    string requestQueueName = configuration["RabbitMqSettings:RequestQueueName"];

    int port = int.TryParse(configuration["RabbitMqSettings:Port"], out var parsedPort) ? parsedPort : 5672;

    return new RabbitMqConfig(hostName, requestQueueName, port);
});

builder.Services.AddHostedService<RabbitMqServerService>();

builder.Services.AddSingleton(serviceProvider =>
{
    var rabbitMqConfig = serviceProvider.GetRequiredService<RabbitMqConfig>();
    return rabbitMqConfig.GetChannel();
});

builder.Services.AddMediatR(typeof(CreateClienRequestHandler).Assembly);
builder.Services.AddAutoMapper(typeof(CreateClientMapper));
builder.Services.AddScoped<Resources>();
builder.Services.AddLocalization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI(Style.Dark);
}

app.UseSerilogRequestLogging();

app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); 
}

app.Run();
