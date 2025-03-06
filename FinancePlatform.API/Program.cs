using FinancePlatform.API.Application.Mapper;
using FinancePlatform.API.Infrastructure.Messaging;
using FinancePlatform.API.Infrastructure.Configurations;
using FinancePlatform.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
// Adiciona os serviços da aplicação (Use Cases, Repositórios, Validators, etc.)
builder.Services.AddApplicationDependencies();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


//builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddApplicationDependencies();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddCacheConfiguration(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.RegisterMaps();

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

app.Run();
