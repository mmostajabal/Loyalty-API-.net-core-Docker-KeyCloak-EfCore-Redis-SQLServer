using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyShared
{
    public static class CommonMethods
    {
        public static bool DateIsNullOrEmpty(DateTime? dateTime)
        {
            return !dateTime.HasValue || dateTime.Value == DateTime.MinValue;
        }
    }
}
