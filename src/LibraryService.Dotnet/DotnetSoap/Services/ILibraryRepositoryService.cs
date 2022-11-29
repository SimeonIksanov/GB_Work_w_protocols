using DotnetSoap.Models;

namespace DotnetSoap.Services;

public interface ILibraryRepositoryService
{
    IList<Book> GetByTitle(string title);

    IList<Book> GetByAuthor(string authorName);

    IList<Book> GetByCategory(string category);

}
