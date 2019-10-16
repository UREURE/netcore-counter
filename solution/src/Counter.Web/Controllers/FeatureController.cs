using Counter.Web.Constantes;
using Counter.Web.Entidades.Configuracion;
using Counter.Web.Loggers;
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
    public class FeatureController : Controller
    {
        #region Declaraciones

        private readonly ILogger logger;
        private readonly AppConfig configuracion;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor básico del Controlador
        /// </summary>
        /// <param name="logger">Logger de la aplicación</param>
        /// <param name="configuracion">Configuración de la aplicación</param>
        public FeatureController(ILogger logger, IOptions<AppConfig> configuracion)
        {
            this.logger = logger;
            this.configuracion = configuracion.Value;
        }

        #endregion

        /// <summary>
        /// Obtiene el valor de activación de la feature
        /// </summary>
        /// <returns>Retorna un error 500</returns>
        [HttpGet]
        [Route("PersistenciaNextCounter")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> PersistenciaNextCounter()
        {
            try
            {
                logger.Trace($"Llamado método PersistenciaNextCounter().");
                bool resultado = await Task.Run(() => { return configuracion.IsFeaturePersistenciaNextCounterEnabled; });
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Activa la feature
        /// </summary>
        /// <returns>Retorna el valor de activación de la feature</returns>
        [HttpGet]
        [Route("PersistenciaNextCounterActivar")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> PersistenciaNextCounterActivar()
        {
            try
            {
                logger.Trace($"Llamado método PersistenciaNextCounterActivar().");
                configuracion.SetFeaturePersistenciaNextCounterEnabled(true);
                bool resultado = await Task.Run(() => { return configuracion.IsFeaturePersistenciaNextCounterEnabled; });
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Desactiva la feature
        /// </summary>
        /// <returns>Retorna el valor de activación de la feature</returns>
        [HttpGet]
        [Route("PersistenciaNextCounterDesactivar")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> PersistenciaNextCounterDesactivar()
        {
            try
            {
                logger.Trace($"Llamado método PersistenciaNextCounterDesactivar().");
                configuracion.SetFeaturePersistenciaNextCounterEnabled(false);
                bool resultado = await Task.Run(() => { return configuracion.IsFeaturePersistenciaNextCounterEnabled; });
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}