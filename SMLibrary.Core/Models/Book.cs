using SMLibraryApp.Core.Models;

namespace SMlibraryApp.Core.Models;

public class Book
{
    public int Id { get; set; }
    public string? Image { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public double? Price { get; set; }
    public string? Content { get; set; }
    public IEnumerable<BookUserName> UserNames = new List<BookUserName>();
}