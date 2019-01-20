using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SmartKitchenApi.Helpers;
using Xunit;

namespace SmartKitchenApi.Tests
{
    public class HelperTests
    {
        private readonly Mock<IRandomNumberGenerator> _randomNumberGenerator = new Mock<IRandomNumberGenerator>();

        public HelperTests(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator.Setup(m => m.RandomNumber(3, 5)).Returns(5);
        }

        [Fact]
        public void RandomNumberGeneratorShouldReturnCorrectRangeTest()
        {
            //Act
                 var results =  _randomNumberGenerator.Setup(m => m.RandomNumber(3, 5)).Returns(4);
            //assert
              Assert.Equal("4", results.ToString());
        }


    }
}
