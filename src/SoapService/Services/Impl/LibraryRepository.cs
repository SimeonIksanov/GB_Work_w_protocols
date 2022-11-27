using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoapService.Services.Impl
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly ILibraryDatabaseContextService _context;

        public LibraryRepository(ILibraryDatabaseContextService context)
        {
            _context = context;
        }

        public IList<Book> GetByAuthor(string author)
        {
            return _context
                .Books
                .Where(book => book.Authors.Any(a => a.Name.ToLower().Contains(author.ToLower())))
                .ToList();
        }

        public IList<Book> GetByCategory(string category)
        {
            return _context
                .Books
                .Where(book => book.Category.ToLower().Contains(category.ToLower()))
                .ToList();
        }

        public IList<Book> GetByTitle(string title)
        {
            return _context
                .Books
                .Where(book => book.Title.ToLower().Contains(title.ToLower()))
                .ToList();
        }
    }
}
