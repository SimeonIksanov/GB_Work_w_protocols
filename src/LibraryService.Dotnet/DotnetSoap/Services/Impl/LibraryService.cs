using System;
using DotnetSoap.Models;
using DotnetSoap.Services;
using DotnetSoap.Services.Impl;

namespace DotnetSoap.Services;

public class LibraryService : ILibraryService
{
    private readonly ILibraryRepositoryService _libraryRepository;

    public LibraryService(ILibraryRepositoryService libraryRepositoryService)
    {
        _libraryRepository = libraryRepositoryService;
    }

    public Book[] GetBookByAuthor(string author)
    {
        return _libraryRepository.GetByAuthor(author).ToArray();
    }

    public Book[] GetBookByCategory(string category)
    {
        return _libraryRepository.GetByCategory(category).ToArray();
    }

    public Book[] GetBookByTitle(string title)
    {
        return _libraryRepository.GetByTitle(title).ToArray();
    }
}

