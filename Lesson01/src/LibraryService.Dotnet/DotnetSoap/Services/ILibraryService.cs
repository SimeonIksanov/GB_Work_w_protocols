using System.ServiceModel;
using DotnetSoap.Models;

namespace DotnetSoap.Services;

[ServiceContract]
public interface ILibraryService
{
    [OperationContract]
    Book[] GetBookByTitle(string s);

    [OperationContract]
    Book[] GetBookByAuthor(string s);

    [OperationContract]
    Book[] GetBookByCategory(string s);

}
