using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMLibrary.Core.Models
{
    public class UserBalance
    {
        public int UserBalanceId { get; set; }
        public string UserName { get; set; }

        public double Balance { get; set; } = 0;
    }
}