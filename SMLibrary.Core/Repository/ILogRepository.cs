using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Core.Repository
{
    public interface ILogRepository
    {
        public  Task<int> CreateLog(Log log);
    }
}