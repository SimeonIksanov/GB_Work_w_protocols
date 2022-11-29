using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoapService.Services;
using SoapService.Services.Impl;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class Repository
    {
        [TestMethod]
        public void GetByCategory_returns_not_empty_collection()
        {
            var repository = CreateRepository();

            var books = repository.GetByCategory("спорт");

            Assert.IsNotNull(books);
            Assert.IsInstanceOfType(books, typeof(IList<Book>));
            Assert.IsTrue(books.Count > 0);
        }

        [TestMethod]
        public void GetByTitle_returns_not_empty_collection()
        {
            var repository = CreateRepository();

            var books = repository.GetByTitle("хоккей");

            Assert.IsNotNull(books);
            Assert.IsInstanceOfType(books, typeof(IList<Book>));
            Assert.IsTrue(books.Count > 0);
        }

        [TestMethod]
        public void GetByAuthor_returns_not_empty_collection()
        {
            var repository = CreateRepository();

            var books = repository.GetByAuthor("максим");

            Assert.IsNotNull(books);
            Assert.IsInstanceOfType(books, typeof(IList<Book>));
            Assert.IsTrue(books.Count > 0);
        }

        private static LibraryRepository CreateRepository()
        {
            ILibraryDatabaseContextService context = new LibraryDatabaseContext();
            LibraryRepository repository = new LibraryRepository(context);
            return repository;
        }
    }
}
