namespace SMLibrary.Core.Models;

public class UserCustomUser
{
    public int UserCustomUserId { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public string? BannerColor { get; set; }
    public string? Description { get; set; }
    IEnumerable<string> Comments = new List<string>();
}
