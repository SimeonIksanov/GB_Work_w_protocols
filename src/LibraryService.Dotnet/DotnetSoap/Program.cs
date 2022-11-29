using DotnetSoap.Services;
using DotnetSoap.Services.Impl;
using SoapCore;

namespace DotnetSoap;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSingleton<ILibraryDatabaseContextService, LibraryDatabaseContext>();
        builder.Services.AddScoped<ILibraryRepositoryService, LibraryRepository>();
        builder.Services.AddScoped<ILibraryService, LibraryService>();
        builder.Services.AddMvc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.UseSoapEndpoint<ILibraryService>(
                "/LibraryService.asmx",
                new SoapEncoderOptions() { },
                SoapSerializer.XmlSerializer);
        });

        app.Run();
    }
}

