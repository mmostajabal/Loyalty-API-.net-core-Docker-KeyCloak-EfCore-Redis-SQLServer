using LoyaltyModels;
using LoyaltyServices.Contracts.Redis;
using LoyaltyServices.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyServices.Services.RedisService
{
    public class RedisSrv : IRedisService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;

        public RedisSrv(ApplicationDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;

        }
        public async Task<string?> GetUserPointsAsync(int userId)
        {
            string cacheKey = $"point_{userId}";
            
            await SetUserPointsAsync(userId, _dbContext.TransactionPoints.Where(o=>o.UserId == userId).Sum(p => p.Points));

            return _cache.GetString(cacheKey);
        }

        public async Task SetUserPointsAsync(int userId, int points)
        {
            string cacheKey = $"point_{userId}";

            await _cache.SetStringAsync(cacheKey, points.ToString());
        }
    }
}
