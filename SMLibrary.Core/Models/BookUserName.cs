using SMlibraryApp.Core.Models;

namespace SMLibraryApp.Core.Models;
public class BookUserName
{
    public int BookUserNameId { get; set; }
    public string UserName { get; set; }

    public int BookId { get; set; }
    public  Book Book { get; set; }
}
