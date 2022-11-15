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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Meme Maker API", Version = "v0.9.0" });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "WebAPI.xml");
    c.IncludeXmlComments(filePath);
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

app.Run();
