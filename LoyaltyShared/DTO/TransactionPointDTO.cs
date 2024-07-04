using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyShared.DTO
{
    public class TransactionPointDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;
        //public UserDTO User { get; set; }
    }
}
