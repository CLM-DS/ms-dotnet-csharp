using Basket.FunctionalTests.Base;
using Microsoft.Microservices.Services.Basket.API.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Basket.FunctionalTests
{
    public class BasketScenarios
        : BasketScenarioBase
    {
        [Fact]
        public async Task Post_basket_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var content = new StringContent(BuildBasket(), UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                   .PostAsync(Post.Basket, content);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_basket_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                   .GetAsync(Get.GetBasket(1));

                response.EnsureSuccessStatusCode();
            }
        }

        string BuildBasket()
        {
            var order = new CustomerBasket(AutoAuthorizeMiddleware.IDENTITY_ID);

            order.Items.Add(new BasketItem
            {
                ProductId = 1,
                ProductName = ".NET Bot Black Hoodie",
                UnitPrice = 10,
                Quantity = 1
            });

            return JsonConvert.SerializeObject(order);
        }

        string BuildCheckout()
        {
            var checkoutBasket = new 
            {
                City = "city",
                Street = "street",
                State = "state",
                Country = "coutry",
                ZipCode = "zipcode",
                CardNumber = "1234567890123456",
                CardHolderName = "CardHolderName",
                CardExpiration = DateTime.UtcNow.AddDays(1),
                CardSecurityNumber = "123",
                CardTypeId = 1,
                Buyer = "Buyer",
                RequestId = Guid.NewGuid()
            };

            return JsonConvert.SerializeObject(checkoutBasket);
        }
    }
}
