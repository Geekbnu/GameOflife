using StackExchange.Redis;
using GameOfLife.Controllers;
using GameOfLife.Domain.Interfaces;
using GameOfLife.Persistence;
using GameOfLife.Domain;
using GameOfLife.Application.Services;

namespace GameOfLifeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = "redis:6379,password=Abc.2024,abortConnect=false";

            var redis = ConnectionMultiplexer.Connect(connectionString);

            //TODO: Rever os tipos Singleton aqui
            builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
            builder.Services.AddScoped<IGameOfLifeEngine, GameOfLifeEngine>();
            builder.Services.AddScoped<IPersistence,RedisCache>();
            builder.Services.AddScoped<IGameOfLifeService, GameOfLIfeService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }

    
}