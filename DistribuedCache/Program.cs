using DistributedCache.Middlewares;
using DistributedCache.Repositories;
using DistributedCache.Repositories.Interfaces;
using DistributedCache.Services;
using DistributedCache.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("redis");
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
