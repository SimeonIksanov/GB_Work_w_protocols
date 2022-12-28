
using NLog.Web;
using SampleService.Extensions;

namespace SampleService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        #region Configure Logging

        builder.Logging
            .ClearProviders()
            .AddConsole()
            .AddNLogWeb(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });
        builder.Services.ConfigureLogging();

        #endregion

        #region Configure HttpClientFactory

        builder.Services.ConfigureHttpFactory();

        #endregion

        builder.Services.AddControllers();

        #region Configure Swagger

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #endregion

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.UseHttpLogging();

        app.MapControllers();

        app.Run();
    }
}

