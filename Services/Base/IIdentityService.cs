using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SMlibraryApp.Services.Base;
public interface IIdentityService
{
    public bool IUserExsists(HttpContext httpContext);
}
