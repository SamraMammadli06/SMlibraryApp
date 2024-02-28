using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Core.Repository
{
    public interface ILogRepository
    {
        public  Task CreateLog(Log log);
    }
}