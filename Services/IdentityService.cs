using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Services.Base;

namespace SMlibraryApp.Services;
public class IdentityService : IIdentityService
{
    private readonly IDataProtector dataProtector;

    public IdentityService(IDataProtectionProvider dataProtectionProvider)
    {
        this.dataProtector = dataProtectionProvider.CreateProtector("Key");
    }

    public IActionResult GetUserId(ref long userId, HttpContext httpContext)
    {
        var authorizeCookie = httpContext.Request.Cookies["Authorize"];

        if (string.IsNullOrWhiteSpace(authorizeCookie))
        {
            return new UnauthorizedResult();
        }

        string? userHashValue = null;
        try
        {
            userHashValue = this.dataProtector.Unprotect(authorizeCookie);
        }
        catch (Exception ex)
        {
            return new BadRequestResult();
        }

        return new OkResult();
    }
}
