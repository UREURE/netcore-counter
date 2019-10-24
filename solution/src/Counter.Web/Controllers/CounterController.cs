using Counter.Web.Constantes;
using Counter.Web.Entidades.Configuracion;
using Counter.Web.Loggers;
using Counter.Web.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Counter.Web.Controllers
{
    /// <summary>
    /// Controlador del contador
    /// </summary>
    [ApiController]
    [Route("/" + UriPath.PREFIX + "/[controller]")]
    public class CounterController : Controller
    {
        #region Declaraciones

        private readonly ILogger logger;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly AppConfig configuracion;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor básico del Controlador
        /// </summary>
        /// <param name="logger">Logger de la aplicación</param>
        /// <param name="configuracion">Configuración de la aplicación</param>
        /// <param name="repositoryFactory">Factoría para obtener el repositorio de las operaciones del controlador</param>
        public CounterController(ILogger logger, IOptions<AppConfig> configuracion, IRepositoryFactory repositoryFactory)
        {
            this.logger = logger;
            this.configuracion = configuracion.Value;
            this.repositoryFactory = repositoryFactory;
        }

        #endregion

        #region Métodos Protegidos

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected ICounterRepository GetRepository()
        {
            if (configuracion != null && configuracion.IsFeaturePersistenciaNextCounterEnabled)
                return repositoryFactory.Get(Claves.SELECTOR_PERSISTENCIA_NEXT_COUNTER);
            else
                return repositoryFactory.Get(Claves.SELECTOR_PERSISTENCIA_REDIS);
        }

        #endregion

        /// <summary>
        /// Obtiene el valor del contador
        /// </summary>
        /// <returns>Retorna el valor del contador</returns>
        [HttpGet]
        [Route("leer")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> Leer()
        {
            try
            {
                logger.Trace($"Llamado método Leer().");
                int contador = await GetRepository().ObtenerContador();
                return Ok(contador);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Incrementa el valor del contador y obtiene el valor del contador incrementado
        /// </summary>
        /// <returns>Retorna el valor del contador después de ser incrementado</returns>
        [HttpGet]
        [Route("incrementar")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> Incrementar()
        {
            try
            {
                logger.Trace($"Llamado método Incrementar().");
                int contador = await GetRepository().IncrementarContador();
                return Ok(contador);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Genera un error en la aplicación para probar el sistema de Logs
        /// </summary>
        /// <returns>Retorna un error 500</returns>
        [HttpGet]
        [Route("error")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> Error()
        {
            try
            {
                logger.Trace($"Llamado método Error().");
                await Task.Run(() => { throw new Exception($"Error publicado."); });
                return Ok(true);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}