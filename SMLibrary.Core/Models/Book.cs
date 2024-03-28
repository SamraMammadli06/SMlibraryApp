using System.Diagnostics.Tracing;
using SMLibrary.Core.Models;
using SMLibraryApp.Core.Models;

namespace SMlibraryApp.Core.Models;

public class Book
{
    public int Id { get; set; }
    public string? Image { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Author { get; set; }
    public string? Content { get; set; }
    public bool IsFinished { get; set; }
    public Tag? tag { get; set; }
    public List<Comment> Comments = new List<Comment>();
    public IEnumerable<BookUserName> UserNames = new List<BookUserName>();
}