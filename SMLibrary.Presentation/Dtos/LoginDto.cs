namespace SMlibraryApp.Presentation.Dtos;
public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? ReturnUrl { get; internal set; }
}
