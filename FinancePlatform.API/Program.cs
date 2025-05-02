using FinancePlatform.API.Infrastructure.Messaging;
using FinancePlatform.API.Infrastructure.Configurations;
using FinancePlatform.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FinancePlatform.API.Infrastructure.Configurations.Mapper;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMySqlConfiguration(builder.Configuration)
    .AddRedisCacheConfiguration(builder.Configuration)
    .AddSwaggerConfiguration();

builder.Services.AddApplicationDependencies();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.RegisterMaps();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
