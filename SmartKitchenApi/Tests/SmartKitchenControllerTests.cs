using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartKitchenApi.Controllers;
using SmartKitchenApi.Helpers;
using Xunit;
using Moq;
using SmartKitchenApi;

namespace SmartKitchenApi.Tests
{
    public class SmartKitchenControllerTest
    {
       /// private readonly IRandomNumberGenerator _randomNumberGenerator;
       /// 
        //private readonly Mock<IRandomNumberGenerator> _randomNumberGenerator = new Mock<IRandomNumberGenerator>();
        //public readonly Mock<ApplicationDbContext> MContext = new Mock<ApplicationDbContext>();
        //private readonly SmartKitchenController _smartKitchenController;


        public SmartKitchenControllerTest()
        {
            //_randomNumberGenerator.Setup(m => m.RandomNumber(3, 5)).Returns(5);
           // _smartKitchenController = new SmartKitchenController(MContext, _randomNumberGenerator);

        }


        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
           // var okResult = _smartKitchenController.Get();

            // Assert
            //Assert.IsType<OkObjectResult>(okResult);
        }
    }


}
