using System.Collections.Generic;
using SoapService.Services.Impl;

namespace SoapService.Services
{
    public interface ILibraryDatabaseContextService
    {
        IList<Book> Books { get; }
    }
}
