using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Dtos;

namespace SMlibraryApp.Controllers;
public class IdentityController :Controller
{
    [HttpGet]
    public async Task<IActionResult> Login(){
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginDto login){
        var schemes = CookieAuthenticationDefaults.AuthenticationScheme;
        return Ok();
    }
}