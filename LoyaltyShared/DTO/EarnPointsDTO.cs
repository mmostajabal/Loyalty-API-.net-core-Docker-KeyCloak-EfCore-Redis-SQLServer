using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyShared.DTO
{
    public class EarnPointsDTO
    {
        public int Points { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}
