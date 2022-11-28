using DotnetSoap.Models;

namespace DotnetSoap.Services;

public interface ILibraryDatabaseContextService
{
    IList<Book> Books { get; }
}
