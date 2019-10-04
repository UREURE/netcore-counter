using Counter.Web.Controllers;
using Counter.Web.Loggers;
using Counter.Web.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Counter.Web.Test.Controllers
{
    public class CounterControllerTest : BaseTest
    {
        [Fact]
        public async Task Leer_OK_200()
        {
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ICounterRepository> repositoryMock = new Mock<ICounterRepository>();

            ActionResult<int> resultadoEsperado = new OkObjectResult(CODIGO_RAMDOM_1);
            repositoryMock.Setup(m => m.ObtenerContador()).Returns(Task.FromResult(CODIGO_RAMDOM_1));

            CounterController counterController = new CounterController(loggerMock.Object, repositoryMock.Object);
            ActionResult<int> resultado = await counterController.Leer();

            resultado.Should().BeEquivalentTo(resultadoEsperado);
        }

        [Fact]
        public void Leer_KO()
        {
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ICounterRepository> repositoryMock = new Mock<ICounterRepository>();

            repositoryMock.Setup(m => m.ObtenerContador()).Throws<Exception>();

            CounterController counterController = new CounterController(loggerMock.Object, repositoryMock.Object);
            Assert.ThrowsAsync<Exception>(() => counterController.Leer());
        }

        [Fact]
        public async Task Incrementar_OK_200()
        {
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ICounterRepository> repositoryMock = new Mock<ICounterRepository>();

            ActionResult<int> resultadoEsperado = new OkObjectResult(CODIGO_RAMDOM_1);
            repositoryMock.Setup(m => m.IncrementarContador()).Returns(Task.FromResult(CODIGO_RAMDOM_1));

            CounterController counterController = new CounterController(loggerMock.Object, repositoryMock.Object);
            ActionResult<int> resultado = await counterController.Incrementar();

            resultado.Should().BeEquivalentTo(resultadoEsperado);
        }

        [Fact]
        public void Incrementar_KO()
        {
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ICounterRepository> repositoryMock = new Mock<ICounterRepository>();

            repositoryMock.Setup(m => m.IncrementarContador()).Throws<Exception>();

            CounterController counterController = new CounterController(loggerMock.Object, repositoryMock.Object);
            Assert.ThrowsAsync<Exception>(() => counterController.Incrementar());
        }

        [Fact]
        public void Error()
        {
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ICounterRepository> repositoryMock = new Mock<ICounterRepository>();

            CounterController counterController = new CounterController(loggerMock.Object, repositoryMock.Object);
            Assert.ThrowsAsync<Exception>(() => counterController.Error());
        }
    }
}