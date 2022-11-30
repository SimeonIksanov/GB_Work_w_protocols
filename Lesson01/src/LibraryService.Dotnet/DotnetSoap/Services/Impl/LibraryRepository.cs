using DotnetSoap.Models;

namespace DotnetSoap.Services.Impl;

public class LibraryRepository : ILibraryRepositoryService
{
    private readonly ILibraryDatabaseContextService _context;

    public IList<Book> GetByTitle(string title)
    {
        try
        {
            return _context.Books.Where(book => book.Title.ToLower().Contains(title.ToLower())).ToList();
        }
        catch
        {
            return new List<Book>();
        }
    }

    public LibraryRepository(ILibraryDatabaseContextService context)
    {
        _context = context;
    }

    public IList<Book> GetByAuthor(string authorName)
    {
        try
        {
            return _context.Books.Where(book =>
                book.Authors.Where(author => author.Name.ToLower().Contains(authorName.ToLower())).Count() > 0).ToList();
        }
        catch
        {
            return new List<Book>();
        }

    }

    public IList<Book> GetByCategory(string category)
    {
        try
        {
            return _context.Books.Where(book => book.Category.ToLower().Contains(category.ToLower())).ToList();
        }
        catch
        {
            return new List<Book>();
        }
    }


}