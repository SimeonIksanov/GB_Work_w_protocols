using System.Diagnostics;
using LibraryService.Web.Models;
using LibraryServiceReference;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Web.Controllers;
public class LibraryController : Controller
{
    private readonly ILogger<LibraryController> _logger;

    public LibraryController(ILogger<LibraryController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> IndexAsync(SearchType searchType, string searchString)
    {
        LibraryWebServiceSoapClient client =
            new LibraryWebServiceSoapClient(LibraryWebServiceSoapClient.EndpointConfiguration.LibraryWebServiceSoap12);

        try
        {
            if (!string.IsNullOrWhiteSpace(searchString) && searchString.Length >= 3)
            {
                switch (searchType)
                {
                    case SearchType.Title:
                        return View(new BookCategoryViewModel
                        {
                            Books = (await client.GetBooksByTitleAsync(searchString)).Body.GetBooksByTitleResult
                            //Books = client.GetBooksByTitle(searchString);
                        });
                    case SearchType.Author:
                        return View(new BookCategoryViewModel
                        {
                            Books = (await client.GetBooksByAuthorAsync(searchString)).Body.GetBooksByAuthorResult
                            //Books = client.GetBooksByAuthor(searchString)
                        });
                    case SearchType.Category:
                        return View(new BookCategoryViewModel
                        {
                            Books = (await client.GetBooksByCategoryAsync(searchString)).Body.GetBooksByCategoryResult
                            //Books = client.GetBooksByCategory(searchString)
                        });
                }

            }
        }
        catch
        {
        }
        return View(new BookCategoryViewModel
        {
            Books = Array.Empty<Book>()
        });
    }

}
