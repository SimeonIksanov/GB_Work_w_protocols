using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SoapService.Services;
using SoapService.Services.Impl;

namespace SoapService
{
    /// <summary>
    /// Summary description for LibraryWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LibraryWebService : System.Web.Services.WebService
    {
        private ILibraryRepository _libraryRepository;

        public LibraryWebService()
        {
            _libraryRepository = new LibraryRepository(new LibraryDatabaseContext());
        }

        [WebMethod]
        public Book[] GetBooksByTitle(string title)
        {
            return _libraryRepository.GetByTitle(title).ToArray();
        }

        [WebMethod]
        public Book[] GetBooksByAuthor(string author)
        {
            return _libraryRepository.GetByAuthor(author).ToArray();
        }

        [WebMethod]
        public Book[] GetBooksByCategory(string category)
        {
            return _libraryRepository.GetByCategory(category).ToArray();
        }


    }
}
