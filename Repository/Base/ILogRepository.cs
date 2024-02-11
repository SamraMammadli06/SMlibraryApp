using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMlibraryApp.Models;

namespace SMlibraryApp.Repository.Base
{
    public interface ILogRepository
    {
        public  Task<int> CreateLog(Log log);
    }
}