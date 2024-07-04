using AutoMapper;
using LoyaltyServices.Contracts.User;
using LoyaltyServices.Data;
using LoyaltyShared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyServices.Services.UserService
{
    public class UserSrv : IUser
    {
        private readonly ApplicationDbContext _dbContext ;
        private readonly IMapper _mapper ;
        public UserSrv(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ;
            _mapper = mapper ;
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDTO Get(int id)
        {
            return _dbContext.Users.Where(u => u.Id == id).Select(u => _mapper.Map<UserDTO>(u)).SingleOrDefault() ?? new UserDTO();
        }
    }
}
