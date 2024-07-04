using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyServices.Contracts.Redis
{
    public interface IRedisService
    {
        Task<String?> GetUserPointsAsync(int userId);
        Task SetUserPointsAsync(int userId, int points);
    }
}
