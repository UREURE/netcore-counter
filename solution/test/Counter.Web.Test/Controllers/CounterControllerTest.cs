using Counter.Web.Constantes;
using Counter.Web.Controllers;
using Counter.Web.Entidades.Configuracion;
using Counter.Web.Loggers;
using Counter.Web.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Counter.Web.Test.Controllers
{
    public class CounterControllerTest : BaseTest
    {
        protected AppConfig ObtenerConfiguracionPersistenciaNextCounterDesactivada()
        {
            AppConfig configuracion = new AppConfig()
            {
                FeatureManagement = new FeatureManagementConfig()
                {
                    PersistenciaNextCounter = false
                }
            };
            return configuracion;
        }

        [Fact]
        public async Task Leer_OK_200()
        {
            AppConfig configuracion = ObtenerConfiguracionPersistenciaNextCounterDesactivada();

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ICounterRepository> repositoryMock = new Mock<ICounterRepository>();
            Mock<IOptions<AppConfig>> optionsMock = new Mock<IOptions<AppConfig>>();
            Mock<IRepositoryFactory> repositoryFactoryMock = new Mock<IRepositoryFactory>();

            ActionResult<int> resultadoEsperado = new OkObjectResult(CODIGO_RAMDOM_1);
            repositoryMock.Setup(m => m.ObtenerContador()).Returns(Task.FromResult(CODIGO_RAMDOM_1));
            optionsMock.Setup(m => m.Value).Returns(configuracion);
            repositoryFactoryMock.Setup(m => m.Get(Claves.SELECTOR_PERSISTENCIA_REDIS)).Returns(repositoryMock.Object);

            CounterController counterController = new CounterController(loggerMock.Object, optionsMock.Object, repositoryFactoryMock.Object);
            ActionResult<int> resultado = await counterController.Leer();

            resultado.Should().BeEquivalentTo(resultadoEsperado);
        }

        [Fact]
        public void Leer_KO()
        {
            AppConfig configuracion = ObtenerConfiguracionPersistenciaNextCounterDesactivada();

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ICounterRepository> repositoryMock = new Mock<ICounterRepository>();
            Mock<IOptions<AppConfig>> optionsMock = new Mock<IOptions<AppConfig>>();
            Mock<IRepositoryFactory> repositoryFactoryMock = new Mock<IRepositoryFactory>();

            repositoryMock.Setup(m => m.ObtenerContador()).Throws<Exception>();
            optionsMock.Setup(m => m.Value).Returns(configuracion);
            repositoryFactoryMock.Setup(m => m.Get(Claves.SELECTOR_PERSISTENCIA_REDIS)).Returns(repositoryMock.Object);

            CounterController counterController = new CounterController(loggerMock.Object, optionsMock.Object, repositoryFactoryMock.Object);
            Assert.ThrowsAsync<Exception>(() => counterController.Leer());
        }

        [Fact]
        public async Task Incrementar_OK_200()
        {
            AppConfig configuracion = ObtenerConfiguracionPersistenciaNextCounterDesactivada();

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ICounterRepository> repositoryMock = new Mock<ICounterRepository>();
            Mock<IOptions<AppConfig>> optionsMock = new Mock<IOptions<AppConfig>>();
            Mock<IRepositoryFactory> repositoryFactoryMock = new Mock<IRepositoryFactory>();

            ActionResult<int> resultadoEsperado = new OkObjectResult(CODIGO_RAMDOM_1);
            repositoryMock.Setup(m => m.IncrementarContador()).Returns(Task.FromResult(CODIGO_RAMDOM_1));
            optionsMock.Setup(m => m.Value).Returns(configuracion);
            repositoryFactoryMock.Setup(m => m.Get(Claves.SELECTOR_PERSISTENCIA_REDIS)).Returns(repositoryMock.Object);

            CounterController counterController = new CounterController(loggerMock.Object, optionsMock.Object, repositoryFactoryMock.Object);
            ActionResult<int> resultado = await counterController.Incrementar();

            resultado.Should().BeEquivalentTo(resultadoEsperado);
        }

        [Fact]
        public void Incrementar_KO()
        {
            AppConfig configuracion = ObtenerConfiguracionPersistenciaNextCounterDesactivada();

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<ICounterRepository> repositoryMock = new Mock<ICounterRepository>();
            Mock<IOptions<AppConfig>> optionsMock = new Mock<IOptions<AppConfig>>();
            Mock<IRepositoryFactory> repositoryFactoryMock = new Mock<IRepositoryFactory>();

            repositoryMock.Setup(m => m.IncrementarContador()).Throws<Exception>();
            optionsMock.Setup(m => m.Value).Returns(configuracion);

            CounterController counterController = new CounterController(loggerMock.Object, optionsMock.Object, repositoryFactoryMock.Object);
            Assert.ThrowsAsync<Exception>(() => counterController.Incrementar());
        }

        [Fact]
        public void Error()
        {
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IOptions<AppConfig>> optionsMock = new Mock<IOptions<AppConfig>>();
            Mock<IRepositoryFactory> repositoryFactoryMock = new Mock<IRepositoryFactory>();

            CounterController counterController = new CounterController(loggerMock.Object, optionsMock.Object, repositoryFactoryMock.Object);
            Assert.ThrowsAsync<Exception>(() => counterController.Error());
        }
    }
}