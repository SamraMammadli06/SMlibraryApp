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

    public bool IUserExsists(HttpContext httpContext)
    {
        var authorizeCookie = httpContext.Request.Cookies["Authorize"];

        if (string.IsNullOrWhiteSpace(authorizeCookie))
        {
            return false;
        }
        return true;
    }
}
