using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyModels
{
    public class User
    {
       
        public int Id { get; set; }       
        public string Username { get; set; } = String.Empty;
        public ICollection<TransactionPoint> TransactionPoints { get; set; }
    }
}
