using Counter.Web.Constantes;
using Counter.Web.Loggers;
using Counter.Web.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Counter.Web.Controllers
{
    /// <summary>
    ///
    /// </summary>
    //[ApiController]
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
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="counterRepository"></param>
        public CounterController(ILogger logger, ICounterRepository counterRepository)
        {
            this.logger = logger;
            this.counterRepository = counterRepository;
            publicarError = true;
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("leer")]
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
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("incrementar")]
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
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("error")]
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