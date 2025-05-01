using FinancePlatform.API.Application.Mapper;
using FinancePlatform.API.Infrastructure.Messaging;
using FinancePlatform.API.Infrastructure.Configurations;
using FinancePlatform.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDependencies();

var connectionString = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "RedisInstance";
    options.Configuration = redisConnection;
});

builder.Services.AddApplicationDependencies();
builder.Services.AddSwaggerConfiguration();
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
