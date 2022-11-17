using Application.Managers;
using Application.Managers.Interfaces;
using Application.Mappings;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using WebAPI.Swagger;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Setting up Logging Service

Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Async(a => a.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}"))
                    .WriteTo.Async(a => a.MSSqlServer(connectionString: builder.Configuration.GetConnectionString("MemeMakerConnection"), tableName:"Logs", autoCreateSqlTable: true, restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Warning))
                    .WriteTo.Async(a => a.File(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, flushToDiskInterval: new TimeSpan(0,0,10), path:builder.Configuration["DiskPaths:LogFilePath"], rollOnFileSizeLimit:true, fileSizeLimitBytes: 50100100, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5, buffered: false, outputTemplate: "{Timestamp:dd-MMM-yyyy HH:mm:ss.fff zzz} {Level:us} tid={ThreadId} {Message:lj}{NewLine}"))
                    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Meme Maker API", Version = "v0.9.0" });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "WebAPI.xml");
    c.IncludeXmlComments(filePath);

    c.OperationFilter<AddApiKeyHeader>();
}); 

#region [ DATABASE ]

var conn = builder.Configuration.GetConnectionString("MemeMakerConnection");
builder.Services.AddDbContext<MemeMakerDBContext>(options => options.UseSqlServer(conn));

#endregion

#region [ MAPPER ]

builder.Services.AddSingleton(AutoMapperConfig.Initialize());

#endregion

#region [ REPOSITORIES ]

builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
builder.Services.AddScoped<ITemplateUsageRepository, TemplateUsageRepository>();
builder.Services.AddScoped<IMemeRepository, MemeRepository>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();

#endregion

#region [ MANAGERS ]

builder.Services.AddScoped<ITemplateManager, TemplateManager>();
builder.Services.AddScoped<IMemeManager, MemeManager>();
builder.Services.AddScoped<IApiKeyManager, ApiKeyManager>();

#endregion

#region [ UTILS ]

builder.Services.AddSingleton(Random.Shared);

#endregion

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

//app.UseDeveloperExceptionPage();

try
{
    Log.Warning("API is starting...");
    app.Run();
}
catch(Exception e)
{
    Log.Fatal(e, "Exception during startup or execution of API");
}
finally
{
    Log.Warning("API is closing...");
    Log.CloseAndFlush();
}
