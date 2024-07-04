using Azure.Core;
using LoyaltyServices.Contracts.Redis;
using LoyaltyServices.Contracts.TransactionPoint;
using LoyaltyServices.Contracts.User;
using LoyaltyShared.DTO;
using LoyaltyShared.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using System.Text;

namespace LoyaltyAPI.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _userSrv;
        private readonly ITransactionPoint _transactionPointSrv;
        private readonly IRedisService _redisService;

        public UsersController(IUser userSrv, ITransactionPoint transactionPointSrv, IRedisService redisService)
        {
            _userSrv = userSrv;
            _transactionPointSrv = transactionPointSrv;
            _redisService = redisService;
        }

        /// <summary>
        /// Earn
        /// </summary>
        /// <param name="id"></param>
        /// <param name="earnPoints"></param>
        /// <returns></returns>
        [HttpPost("{id}/earn")]
        [Authorize]
        public async Task<IActionResult> Earn(int id, [FromBody] EarnPointsDTO earnPoints)
        {
            var validator = new EarnPointsValidator();
            var validationResult = validator.Validate(earnPoints);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                if (_userSrv.Get(id).Id == 0)
                {
                    Log.Error($"Earn API We could not find user by id {id}");
                    return NotFound();

                }

                TransactionPointDTO result = _transactionPointSrv.InsetTransactionPoint(id, earnPoints);

                // Cache the points
                

                 _redisService.SetUserPointsAsync(id, earnPoints.Points).GetAwaiter().GetResult();

                Log.Information($"Insert Data Id: {result.Id} Points : {result.Points} UserId : {result.UserId}  Sum : {_redisService.GetUserPointsAsync(id).GetAwaiter().GetResult()}");

                return Ok(result);
            }catch (Exception ex)
            {
                Log.Error($"Earn API Exception {ex.ToString()}");
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// GetPoints
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getpoints")]
        [Authorize]
        public async Task<IActionResult> GetPoints(int id)
        {
            try
            {
                var t = _userSrv.Get(id);
                if (_userSrv.Get(id) == null)
                {
                    Log.Error($"Get Points We could not find user by id {id}");
                    return NotFound();
                }

                List<TransactionPointDTO> result = _transactionPointSrv.GetPoints(id);

                Log.Information($"Get Points  {result.Count()}");

                return Ok(result);
            }catch(Exception ex)
            {
                Log.Error($"Earn API Exception {ex.ToString()}");
                return BadRequest(ex);
            }
        }

    }
}
