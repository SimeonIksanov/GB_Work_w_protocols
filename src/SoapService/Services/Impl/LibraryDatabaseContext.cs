using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace SoapService.Services.Impl
{
    public class LibraryDatabaseContext : ILibraryDatabaseContextService
    {
        private IList<Book> _libraryDatabase;


        public LibraryDatabaseContext()
        {
            InitializeDatabase();
        }


        public IList<Book> Books => _libraryDatabase;


        private void InitializeDatabase()
        {
            _libraryDatabase = JsonConvert
                .DeserializeObject<IList<Book>>(Encoding.UTF8.GetString(Properties.Resources.books));
        }

    }
}
