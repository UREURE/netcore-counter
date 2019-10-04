using Counter.Web.Constantes;
using Counter.Web.Loggers;
using Counter.Web.Repository;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ICounterRepository counterRepository;

        private readonly bool publicarError;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor básico del Controlador
        /// </summary>
        /// <param name="logger">Logger de la aplicación</param>
        /// <param name="counterRepository">Instancia para las operaciones del controlador</param>
        public CounterController(ILogger logger, ICounterRepository counterRepository)
        {
            this.logger = logger;
            this.counterRepository = counterRepository;
            publicarError = true;
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
                int contador = await counterRepository.ObtenerContador();
                return Ok(contador);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Incrementa el valor del contador
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
                int contador = await counterRepository.IncrementarContador();
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
                if (publicarError)
                    throw new Exception($"Error publicado, variable publicarError={publicarError}.");
                else
                    return await Task.Run(() => { return Ok(true); });
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}