using DatabasePractice.Controllers;
using DatabasePractice.Interfaces;
using DatabasePractice.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UnitTestingOfDatabaseProject
{
    public class UnitTestOrders
    {
        private Mock<IOrdersRepository> _mockRepo;
        private OrdersController _controller;
        private List<Order> _testOrders;
        public UnitTestOrders()
        {
            _mockRepo = new Mock<IOrdersRepository>();
            _controller = new OrdersController(_mockRepo.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            _testOrders = new List<Order> {
                new Order { Id = 1, Сумма = 8800.85m, ДатаИВремя = new DateTime(2023, 10, 01, 10, 30, 0), Статус = "Выполнен", Клиент = 1 },
                new Order { Id = 2, Сумма = 1250.50m, ДатаИВремя = new DateTime(2023, 10, 02, 14, 15, 0), Статус = "В обработке", Клиент = 2  },
                new Order { Id = 3, Сумма = 560.20m,  ДатаИВремя = new DateTime(2023, 10, 03, 09, 45, 0), Статус = "Отменен", Клиент = 3 } 
            };
        }
        [Fact]
        public void GetOrders_ReturnsCorrectOrder_WhenWithoutFilter()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetOrdersFiltered(It.IsAny<string>())).Returns(_testOrders);
            // Act
            var result = _controller.GetOrders(filter: "", page: 1, pageSize: 10);
            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var orders = okResult.Value.Should().BeAssignableTo<List<Order>>().Subject;
            orders.Should().HaveCount(3);
        }
        [Theory]
        [InlineData("8800.85", 1)]
        [InlineData("1250.50", 2)]
        [InlineData("560.20", 3)]
        public void GetOrders_ReturnsCorrectOrder_WhenFilterBySum(string filter, int expectedId)
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetOrdersFiltered(filter)).Returns<string>(c =>
            {
                if (decimal.TryParse(filter, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal sum))
                    return _testOrders.Where(o => o.Сумма == sum).ToList();
                return new List<Order>();
            });
            //Act
            var result = _controller.GetOrders(filter: filter, page: 1, pageSize: 10);
            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var orders = okResult.Value.Should().BeAssignableTo<List<Order>>().Subject;
            orders.Should().ContainSingle();
            orders[0].Id.Should().Be(expectedId);
            orders[0].Сумма.Should().Be(decimal.Parse(filter, CultureInfo.InvariantCulture));
        }
        [Theory]
        [InlineData("Выполнен", 1)]
        [InlineData("В обработке", 2)]
        [InlineData("Отменен", 3)]
        public void GetOrders_ReturnsCorrectOrder_WhenFilterByStatus(string filter, int expectedId)
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetOrdersFiltered(filter))
                .Returns(_testOrders.Where(c => c.Статус.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList());
            //Act
            var result = _controller.GetOrders(filter: filter, page: 1, pageSize: 10);
            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var orders = okResult.Value.Should().BeAssignableTo<List<Order>>().Subject;
            orders.Should().ContainSingle();
            orders[0].Id.Should().Be(expectedId);
            orders[0].Статус.Should().Be(filter);
        }
        [Theory]
        [InlineData("2023-10-01", 1)]
        [InlineData("2023-10-02", 2)]
        [InlineData("2023-10-03", 3)]
        public void GetOrders_ReturnsCorrectOrder_WhenFilterByDateOnly(string filter, int expectedId)
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetOrdersFiltered(filter))
                .Returns<string>(f =>
                {
                    if (DateOnly.TryParseExact(filter, new[] { "dd.MM.yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "dd/MM/yyyy" },
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly dateFilter))
                    {
                        DateTime startDate = dateFilter.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
                        DateTime endDate = dateFilter.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
                        return _testOrders.Where(o => o.ДатаИВремя >= startDate && o.ДатаИВремя <= endDate).ToList();
                    }
                    return new List<Order>();
                });
            //Act
            var result = _controller.GetOrders(filter: filter, page: 1, pageSize: 10);
            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var orders = okResult.Value.Should().BeAssignableTo<List<Order>>().Subject;
            orders.Should().ContainSingle();
            orders[0].Id.Should().Be(expectedId);
            var expectedDate = DateOnly.ParseExact(filter, new[] { "yyyy.MM.dd", "yyyy-MM-dd", "dd.MM.yyyy" },
            CultureInfo.InvariantCulture);
            orders[0].ДатаИВремя.Date.Should().Be(expectedDate.ToDateTime(TimeOnly.MinValue).Date);
        }
    }
}