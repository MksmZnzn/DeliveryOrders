using Castle.Core.Logging;
using DeliveryOrders.Application.CQRS.Queries;
using DeliveryOrders.Domain;
using DeliveryOrders.WebApi.Controllers;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace Tests.DeliveryOrders 
{
    public class GetFilteredOrdersTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<DeliveryOrdersController>> _loggerMock;
        private readonly DeliveryOrdersController _controller;
        
        public GetFilteredOrdersTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<DeliveryOrdersController>>();
            _controller = new DeliveryOrdersController(_mediatorMock.Object, _loggerMock.Object);
        }
        
        // Проверка фильтрации
        [Fact]
        public async Task GetFilteredOrders_ReturnsCorrectFilteredOrders()
        {
            #region Arrange
            var fakeOrders = new List<Order>
            {
                new Order
                {
                    District = "District2",
                    OrderId = Guid.NewGuid(),
                    OrderTime = DateTime.Now.AddMinutes(5),
                    Weight = 2.1
                },
                new Order
                {
                    District = "District1",
                    OrderId = Guid.NewGuid(),
                    OrderTime = DateTime.Now.AddMinutes(10),
                    Weight = 2.5
                },
                new Order
                {
                    District = "District2",
                    OrderId = Guid.NewGuid(),
                    OrderTime = DateTime.Now.AddMinutes(15),
                    Weight = 3.0
                }
            };
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetFilteredOrdersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeOrders.Where(o => o.District == "District2").ToList());

            var query = new GetFilteredOrdersQuery
            {
                District = "District2",
                Time = DateTime.Now.AddMinutes(5)
            };
            #endregion

            #region Act
            var result = await _controller.GetFilteredOrders(query);
            #endregion

            #region Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orders = Assert.IsAssignableFrom<List<Order>>(okResult.Value);
            Assert.Equal(2, orders.Count);
            Assert.All(orders, o => Assert.Equal("District2", o.District));
            #endregion
        }
        
        
        [Fact]
        public async Task GetFilteredOrders_ShouldReturnBadRequest_WhenDistrictIsEmpty()
        {
            #region Arrange
            var query = new GetFilteredOrdersQuery
            {
                District = "",
                Time = DateTime.Now
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetFilteredOrdersQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("District", "Район обязателен для фильтрации.")
                }));
            #endregion


            #region Act
            var result = await _controller.GetFilteredOrders(query);
            #endregion


            #region Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Район обязателен для фильтрации.", badRequestResult.Value.ToString());
            #endregion
        }
        
        [Fact]
        public async Task GetFilteredOrders_ShouldReturnBadRequest_WhenTimeIsInFuture()
        {
            #region Arrange
            var futureTime = DateTime.Now.AddDays(1);
            var query = new GetFilteredOrdersQuery
            {
                District = "District1",
                Time = futureTime
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetFilteredOrdersQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Time", "Время не может быть из будущего.")
                }));
            #endregion

            #region Act
            var result = await _controller.GetFilteredOrders(query);
            #endregion

            #region Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Время не может быть из будущего.", badRequestResult.Value.ToString());
            #endregion
        }
    }
}