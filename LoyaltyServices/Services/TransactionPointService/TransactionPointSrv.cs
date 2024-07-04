using AutoMapper;
using LoyaltyServices.Contracts.TransactionPoint;
using LoyaltyServices.Data;
using LoyaltyServices.Services.UserService;
using LoyaltyShared.DTO;
using LoyaltyShared;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyModels;
using System.Transactions;
using LoyaltyServices.Contracts.User;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LoyaltyServices.Services.TransactionPointService
{
    public class TransactionPointSrv : ITransactionPoint
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public TransactionPointSrv(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            
        }
        /// <summary>
        /// GetPints
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TransactionPointDTO> GetPoints(int id)
        {
            return _dbContext.TransactionPoints.Where(c=>c.UserId == id).Select(map=> _mapper.Map<TransactionPointDTO>(map)).ToList();
        }

        /// <summary>
        /// InsetTransactionPoint
        /// </summary>
        /// <param name="id"></param>
        /// <param name="earnPoints"></param>
        /// <returns></returns>
        public TransactionPointDTO InsetTransactionPoint(int id, EarnPointsDTO earnPoints)
        {
            TransactionPoint transactionPoint = new TransactionPoint();
            transactionPoint.UserId = id;
            transactionPoint.Points = earnPoints.Points;
            transactionPoint.TransactionDate = CommonMethods.DateIsNullOrEmpty(earnPoints.TransactionDate) ? DateTime.Now : earnPoints.TransactionDate;

            _dbContext.Add(transactionPoint);
            _dbContext.SaveChanges();

            return _mapper.Map<TransactionPointDTO>(transactionPoint);
        }
    }
}
