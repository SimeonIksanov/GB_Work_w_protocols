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
            return Fetch(book => book.Authors.Any(a => a.Name.ToLower().Contains(author.ToLower())));
        }

        public IList<Book> GetByCategory(string category)
        {
            return Fetch(book => book.Category.ToLower().Contains(category.ToLower()));
        }

        public IList<Book> GetByTitle(string title)
        {
            return Fetch(book => book.Title.ToLower().Contains(title.ToLower()));
        }

        private IList<Book> Fetch(Func<Book, bool> query)
        {
            try
            {
                return _context
                    .Books
                    .Where(book => query(book))
                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<Book>();
            }
        }
    }
}
