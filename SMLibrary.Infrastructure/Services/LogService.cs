using SMlibraryApp.Core.Services;

namespace SMlibraryApp.Infrastructure.Services;
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
