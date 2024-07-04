using AutoMapper;
using LoyaltyModels;
using LoyaltyShared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyServices.Mapping
{
    public class Maps : Profile
    {

        public Maps()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();

            CreateMap<TransactionPoint, TransactionPointDTO>().ReverseMap();
            CreateMap<TransactionPointDTO, TransactionPoint>().ReverseMap();
        }
    }
}
