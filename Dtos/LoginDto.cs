using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMlibraryApp.Dtos;
public class LoginDto
{
    public string ReturnUrl {  get;  set;  }  
    public string UserName { get; set; }
    public string Password { get; set; }
}