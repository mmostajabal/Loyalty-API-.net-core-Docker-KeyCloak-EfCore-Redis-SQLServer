﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyShared.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public ICollection<TransactionPointDTO> TransactionPoints { get; set; }
    }
}
