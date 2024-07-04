using LoyaltyModels;
using LoyaltyServices.Data;
using LoyaltyShared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyAPITest.MockData
{
    public static class SeedClass
    {
        public static List<User> user;
        public static List<TransactionPoint> transactionPoints;
        public static List<UserDTO> userDTO;
        public static List<TransactionPointDTO> transactionPointsDTO;

        public static void UserInitData(ApplicationDbContext _contextMock)
        {
            user = new List<User>() { new User() {
                Id = 1, Username = "User1", TransactionPoints = new List<TransactionPoint>() }, 
                new User() { Id = 2, Username = "User2", TransactionPoints = new List<TransactionPoint>() } };
            _contextMock.Users.AddRange(user);
        }

        public static void UserDTOInitData()
        {
            userDTO = new List<UserDTO>() { 
                new UserDTO() {Id = 1, Username = "User1", TransactionPoints = new List<TransactionPointDTO>() },
                new UserDTO() { Id = 2, Username = "User2", TransactionPoints = new List<TransactionPointDTO>() } };
        }


        public static void TransactionPointsInitData(ApplicationDbContext _contextMock)
        {

            transactionPoints = new List<TransactionPoint>() {
                    new TransactionPoint(){Id=1, UserId= 1, Points= 10, TransactionDate= DateTime.Now},
                    new TransactionPoint(){Id=2, UserId= 1, Points= 20, TransactionDate= DateTime.Now},
                    new TransactionPoint(){Id=3, UserId= 2, Points= 15, TransactionDate= DateTime.Now},
                    new TransactionPoint(){Id=4, UserId= 2, Points= 5, TransactionDate= DateTime.Now}

            };
            _contextMock.TransactionPoints.AddRange(transactionPoints);
            _contextMock.SaveChanges();

        }

        public static void TransactionPointsDTOInitData()
        {

            transactionPointsDTO = new List<TransactionPointDTO>() {
                    new TransactionPointDTO(){Id=1, UserId= 1, Points= 10, TransactionDate= DateTime.Now},
                    new TransactionPointDTO(){Id=2, UserId= 1, Points= 20, TransactionDate= DateTime.Now},
                    new TransactionPointDTO(){Id=3, UserId= 2, Points= 15, TransactionDate= DateTime.Now},
                    new TransactionPointDTO(){Id=4, UserId= 2, Points= 5, TransactionDate= DateTime.Now}

            };

        }

    }
}
