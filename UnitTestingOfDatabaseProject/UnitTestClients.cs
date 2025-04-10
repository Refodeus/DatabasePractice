using DatabasePractice.Controllers;
using DatabasePractice.Interfaces;
using DatabasePractice.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Globalization;

namespace UnitTestingOfDatabaseProject
{
    public class UnitTestClients
    {
        private Mock<IClientsRepository> _mockRepo;
        private ClientsController _controller;
        private List<Client> _testClients;
        public UnitTestClients()
        {
            _mockRepo = new Mock<IClientsRepository>();
            _controller = new ClientsController(_mockRepo.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            _testClients = new List<Client> {
                new Client { Id = 1, Имя = "Иван", Фамилия = "Голунков", ДатаРождения = new DateOnly(1988, 12, 16) },
                new Client { Id = 2, Имя = "Мария", Фамилия = "Певчих", ДатаРождения = new DateOnly(1998, 2, 10) },
                new Client { Id = 3, Имя = "Александр", Фамилия = "Шариков", ДатаРождения = new DateOnly(2001, 7, 27) }};
        }
        [Fact]
        public void GetClients_ReturnsCorrectClient_WhenWithoutFilter()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetClientsFiltered(It.IsAny<string>())).Returns(_testClients);
            // Act
            var result = _controller.GetClients(filter: "", page: 1, pageSize: 10);
            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var clients = okResult.Value.Should().BeAssignableTo<List<Client>>().Subject;
            clients.Should().HaveCount(3);
        }
        [Theory]
        [InlineData("Иван", 1)]
        [InlineData("Мария", 2)]
        [InlineData("Александр", 3)]
        public void GetClients_ReturnsCorrectClient_WhenFilterByName(string filter, int expectedId)
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetClientsFiltered(filter)).Returns(_testClients.Where(c => 
            c.Имя.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList());
            //Act
            var result = _controller.GetClients(filter: filter, page: 1, pageSize: 10);
            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var clients = okResult.Value.Should().BeAssignableTo<List<Client>>().Subject;
            clients.Should().ContainSingle();
            clients[0].Id.Should().Be(expectedId);
            clients[0].Имя.Should().Be(filter);
        }
        [Theory]
        [InlineData("Голунков", 1)]
        [InlineData("Певчих", 2)]
        [InlineData("Шариков", 3)]
        public void GetClients_ReturnsCorrectClient_WhenFilterBySurname(string filter, int expectedId)
        {
            //Arrange
            _mockRepo.Setup(repo => repo.GetClientsFiltered(filter))
                .Returns(_testClients.Where(c => c.Фамилия.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList());
            //Act
            var result = _controller.GetClients(filter: filter, page: 1, pageSize: 10);
            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var clients = okResult.Value.Should().BeAssignableTo<List<Client>>().Subject;
            clients.Should().ContainSingle();
            clients[0].Id.Should().Be(expectedId);
            clients[0].Фамилия.Should().Be(filter);
        }
        [Theory]
        [InlineData("1988.12.16", 1)]
        [InlineData("1998-02-10", 2)]
        [InlineData("27.07.2001", 3)]
        public void GetClients_ReturnsCorrectClient_WhenFilterByDateOnly(string filter, int expectedId)
        {
            //Arrange
            
            _mockRepo.Setup(repo => repo.GetClientsFiltered(filter))
                .Returns<string>(f =>
                {
                    if (DateOnly.TryParseExact(filter, new[] { "yyyy.MM.dd", "yyyy-MM-dd", "dd.MM.yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateFilter))
                    {
                        return _testClients.Where(c => c.ДатаРождения == dateFilter).ToList();
                    }
                    return new List<Client>();
                });
            //Act
            var result = _controller.GetClients(filter: filter, page: 1, pageSize: 10);
            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var clients = okResult.Value.Should().BeAssignableTo<List<Client>>().Subject;
            clients.Should().ContainSingle();
            clients[0].Id.Should().Be(expectedId);
            var expectedDate = DateOnly.ParseExact(filter, new[] { "yyyy.MM.dd", "yyyy-MM-dd", "dd.MM.yyyy" },
            CultureInfo.InvariantCulture);
            clients[0].ДатаРождения.Should().Be(expectedDate);
        }
        [Theory]
        [InlineData("НесуществующееИмя")]
        [InlineData("НесуществующаяФамилия")]
        [InlineData("3000-01-01")]
        public void GetClients_ReturnsEmpty_WhenNoMatches(string filter)
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetClientsFiltered(filter))
                .Returns<string>(f =>
                {
                    if (DateOnly.TryParseExact(filter, new[] { "yyyy.MM.dd", "yyyy-MM-dd", "dd.MM.yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateFilter))
                    {
                        return _testClients.Where(c => c.ДатаРождения == dateFilter).ToList();
                    }
                    else
                    {
                        return _testClients.Where(c => c.Имя.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                        c.Фамилия.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                });
            // Act
            var result = _controller.GetClients(filter: filter, page: 1, pageSize: 10);
            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var clients = okResult.Value.Should().BeAssignableTo<List<Client>>().Subject;
            clients.Should().BeEmpty();
        }
    }
}