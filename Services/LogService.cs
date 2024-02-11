using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMlibraryApp.Services.Base;

namespace SMlibraryApp.Services;
public class LogService : ILogService
{
    private readonly bool isEnabled;
    public LogService(bool isEnabled)
    {
        this.isEnabled = isEnabled;
    }
    public bool IsLogEnabled()
    {
        return  this.isEnabled;
    }
}
