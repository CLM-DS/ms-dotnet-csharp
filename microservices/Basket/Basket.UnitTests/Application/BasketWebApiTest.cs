using Basket.API.IntegrationEvents.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Microservices.Events.EventBus.Abstractions;
using Microsoft.Microservices.Services.Basket.API.Controllers;
using Microsoft.Microservices.Services.Basket.API.Model;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Basket.Application
{
    public class BasketWebApiTest
    {
        private readonly Mock<IBasketRepository> _basketRepositoryMock;
        private readonly Mock<IEventBus> _serviceBusMock;
        private readonly Mock<ILogger<BasketController>> _loggerMock;

        public BasketWebApiTest()
        {
            _basketRepositoryMock = new Mock<IBasketRepository>();
            _serviceBusMock = new Mock<IEventBus>();
            _loggerMock = new Mock<ILogger<BasketController>>();
        }

        [Fact]
        public async Task Get_customer_basket_success()
        {
            //Arrange
            var fakeCustomerId = "1";
            var fakeCustomerBasket = GetCustomerBasketFake(fakeCustomerId);

            _basketRepositoryMock.Setup(x => x.GetBasketAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeCustomerBasket));

            _serviceBusMock.Setup(x => x.Publish(It.IsAny<UserCheckoutAcceptedIntegrationEvent>()));

            //Act
            var basketController = new BasketController(
                _loggerMock.Object,
                _basketRepositoryMock.Object,
                _serviceBusMock.Object);

            var actionResult = await basketController.GetBasketByIdAsync(fakeCustomerId);

            //Assert
            Assert.Equal((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal((((ObjectResult)actionResult.Result).Value as CustomerBasket).BuyerId, fakeCustomerId);
        }

        [Fact]
        public async Task Post_customer_basket_success()
        {
            //Arrange
            var fakeCustomerId = "1";
            var fakeCustomerBasket = GetCustomerBasketFake(fakeCustomerId);

            _basketRepositoryMock.Setup(x => x.UpdateBasketAsync(It.IsAny<CustomerBasket>()))
                .Returns(Task.FromResult(fakeCustomerBasket));
            _serviceBusMock.Setup(x => x.Publish(It.IsAny<UserCheckoutAcceptedIntegrationEvent>()));

            //Act
            var basketController = new BasketController(
                _loggerMock.Object,
                _basketRepositoryMock.Object,
                _serviceBusMock.Object);

            var actionResult = await basketController.UpdateBasketAsync(fakeCustomerBasket);

            //Assert
            Assert.Equal((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal((((ObjectResult)actionResult.Result).Value as CustomerBasket).BuyerId, fakeCustomerId);
        }

        private CustomerBasket GetCustomerBasketFake(string fakeCustomerId)
        {
            return new CustomerBasket(fakeCustomerId)
            {
                Items = new List<BasketItem>()
                {
                    new BasketItem()
                }
            };
        }
    }
}
