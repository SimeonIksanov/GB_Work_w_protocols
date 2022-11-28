using DotnetSoap.Models;
using System.Reflection;
using System.Resources;
using System.Text.Json;

namespace DotnetSoap.Services.Impl;

public class LibraryDatabaseContext : ILibraryDatabaseContextService
{
    private IList<Book>? _libraryDatabase;

    public IList<Book> Books => _libraryDatabase;


    public LibraryDatabaseContext()
    {
        Initialize();
    }


    private void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using (Stream? stream = assembly.GetManifestResourceStream("DotnetSoap.books.json"))
        {
            if (stream is not null)
                _libraryDatabase = JsonSerializer
                    .Deserialize<List<Book>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

}