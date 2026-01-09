using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RestaurantApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("maui", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<OrderStatusWorker>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("maui");

app.MapControllers();

app.Run();
