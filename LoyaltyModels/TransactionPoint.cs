using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyModels
{
    public class TransactionPoint
    {
       
        public int Id { get; set; }        
        public int UserId { get; set; }
        public int Points { get; set; }
        
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public User User { get; set; }
    }
}
