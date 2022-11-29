using System.ComponentModel.DataAnnotations;

namespace LibraryService.Web.Models;

public enum SearchType
{
    [Display(Name = "Загаловок")]
    Title,
    [Display(Name = "Автор")]
    Author,
    [Display(Name = "Катерогория")]
    Category
}
