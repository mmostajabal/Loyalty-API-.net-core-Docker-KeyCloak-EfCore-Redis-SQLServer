using LoyaltyShared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyServices.Contracts.TransactionPoint
{
    public interface ITransactionPoint
    {
        TransactionPointDTO InsetTransactionPoint(int id, EarnPointsDTO earnPoints);
        List<TransactionPointDTO> GetPoints(int id);
    }
}
