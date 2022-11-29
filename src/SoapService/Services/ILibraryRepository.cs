using System.Collections.Generic;
using SoapService.Services.Impl;

namespace SoapService.Services
{
    public interface ILibraryRepository
    {
        IList<Book> GetByTitle(string title);
        IList<Book> GetByAuthor(string author);
        IList<Book> GetByCategory(string category);
    }
}
