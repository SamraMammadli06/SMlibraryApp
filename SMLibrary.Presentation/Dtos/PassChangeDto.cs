using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMLibrary.Presentation.Dtos;
public class PassChangeDto
{
    public string Username { get; set; }
    public string oldPassword { get; set; }
    public string newPassword { get; set; }
}
