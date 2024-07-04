using AutoMapper;
using LoyaltyAPI.Controllers;
using LoyaltyModels;
using LoyaltyServices.Contracts.Redis;
using LoyaltyServices.Contracts.TransactionPoint;
using LoyaltyServices.Contracts.User;
using LoyaltyServices.Data;
using LoyaltyServices.Mapping;
using LoyaltyServices.Services.TransactionPointService;
using LoyaltyServices.Services.UserService;
using LoyaltyShared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyAPITest.System.Controller
{
    public class UserControllerTest
    {
        Mock<ITransactionPoint> _transactionPointSrv;
        Mock<IUser> _userSrv;
        Mock<IMapper> _mapper;
        Mock<IRedisService> _redisService;
        UsersController _usersController;
        private readonly ApplicationDbContext _dbcontext;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserControllerTest()
        {
            _dbcontext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options);

            MockData.SeedClass.UserInitData(_dbcontext);
            MockData.SeedClass.TransactionPointsInitData(_dbcontext);

            _transactionPointSrv = new Mock<ITransactionPoint>();
            _userSrv = new Mock<IUser>();
            _redisService = new Mock<IRedisService>();

            //var _transactionPoint1Srv = new TransactionPointSrv(_dbcontext, _mapper.Object);
            _mapper = new Mock<IMapper>();

            _usersController = new UsersController(_userSrv.Object, _transactionPointSrv.Object, _redisService.Object);
        }

        /// <summary>
        /// GetPoints_ResultOK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPoints_ResultOK()
        {
            EarnPointsDTO pointsDTO = new EarnPointsDTO();

            //Arrange
            MockData.SeedClass.UserDTOInitData();
            MockData.SeedClass.TransactionPointsDTOInitData();

            UserDTO sampleUserData = MockData.SeedClass.userDTO[0];
            List<TransactionPointDTO> transactionPointDTOs = MockData.SeedClass.transactionPointsDTO.Where(o => o.UserId == sampleUserData.Id).ToList();

            _mapper.Setup(m => m.Map<List<TransactionPointDTO>>(MockData.SeedClass.transactionPoints)).Returns(MockData.SeedClass.transactionPointsDTO);
            _transactionPointSrv.Setup(x => x.GetPoints(It.IsAny<int>())).Returns(transactionPointDTOs);
            _userSrv.Setup(x => x.Get(It.IsAny<int>())).Returns(sampleUserData);
            _redisService.Setup(x => x.SetUserPointsAsync(sampleUserData.Id, 10));

            //Act
            var result = _usersController.GetPoints(1).GetAwaiter().GetResult();
            var okResult = Assert.IsType<OkObjectResult>(result);

            //Assert                
            Assert.Equal(result, okResult);

        }

        /// <summary>
        /// GetPoints_ResultCount
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPoints_ResultCount()
        {
            EarnPointsDTO pointsDTO = new EarnPointsDTO();

            //Arrange
            MockData.SeedClass.UserDTOInitData();
            MockData.SeedClass.TransactionPointsDTOInitData();

            UserDTO sampleUserData = MockData.SeedClass.userDTO[0];
            List<TransactionPointDTO> transactionPointDTOs = MockData.SeedClass.transactionPointsDTO.Where(o => o.UserId == sampleUserData.Id).ToList();

            _mapper.Setup(m => m.Map<List<TransactionPointDTO>>(MockData.SeedClass.transactionPoints)).Returns(MockData.SeedClass.transactionPointsDTO);
            _transactionPointSrv.Setup(x => x.GetPoints(It.IsAny<int>())).Returns(transactionPointDTOs);
            _userSrv.Setup(x => x.Get(It.IsAny<int>())).Returns(sampleUserData);
            _redisService.Setup(x => x.SetUserPointsAsync(sampleUserData.Id, 10));

            //Act
            var result = _usersController.GetPoints(1).GetAwaiter().GetResult();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TransactionPointDTO>>(okResult.Value);

            //Assert                
            Assert.Equal(returnValue.Count, transactionPointDTOs.Count);

        }


        [Fact]
        public async Task Eran_Result()
        {
            DateTime curDate = DateTime.Now;
            EarnPointsDTO earnPoints = new EarnPointsDTO() { Points = 20, TransactionDate = curDate};

            //Arrange
            MockData.SeedClass.UserDTOInitData();
            MockData.SeedClass.TransactionPointsDTOInitData();

            UserDTO sampleUserData = MockData.SeedClass.userDTO[0];
            List<TransactionPointDTO> transactionPointDTOs = MockData.SeedClass.transactionPointsDTO.Where(o => o.UserId == sampleUserData.Id).ToList();

            _mapper.Setup(m => m.Map<List<TransactionPointDTO>>(MockData.SeedClass.transactionPoints)).Returns(MockData.SeedClass.transactionPointsDTO);
            _transactionPointSrv.Setup(x => x.GetPoints(It.IsAny<int>())).Returns(transactionPointDTOs);
            _transactionPointSrv.Setup(x => x.InsetTransactionPoint(It.IsAny<int>(), earnPoints)).Returns(new TransactionPointDTO { Id = 5, Points= earnPoints.Points, TransactionDate = curDate, UserId =sampleUserData.Id}); 
            _userSrv.Setup(x => x.Get(It.IsAny<int>())).Returns(sampleUserData);

            _redisService.Setup(x => x.SetUserPointsAsync(sampleUserData.Id, 10));
            _redisService.Setup(x => x.GetUserPointsAsync(sampleUserData.Id));

            //Act
            var result = _usersController.Earn(sampleUserData.Id, earnPoints);
            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            TransactionPointDTO returnValue = Assert.IsType<TransactionPointDTO>(okResult.Value);

            //Assert                
            Assert.Equal(returnValue.UserId, sampleUserData.Id);
            Assert.Equal(returnValue.Points, earnPoints.Points);
            Assert.Equal(returnValue.TransactionDate, curDate);
        }

    }
}
