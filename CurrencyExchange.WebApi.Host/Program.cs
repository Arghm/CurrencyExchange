using CurrencyExchange.Application.Infrastructure;
using CurrencyExchange.Migrations;
using CurrencyExchange.WebApi.Host.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CurrencyExchange.WebApi.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<CurrencyExchangeContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PgSql"),
                    b => b.MigrationsAssembly("CurrencyExchange.Migrations")));

            builder.Services.AddCurrencyExchangeServices();

            builder.Host.UseSerilog((hbc, cl) => cl.WriteTo.Console());

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}