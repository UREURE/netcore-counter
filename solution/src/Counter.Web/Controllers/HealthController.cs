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
    /// Controlador para readiness liveness
    /// </summary>
    [ApiController]
    [Route("/" + UriPath.PREFIX + "/[controller]")]
    public class HealthController : Controller
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
        public HealthController(ILogger logger, IOptions<AppConfig> configuracion, IRepositoryFactory repositoryFactory)
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
        /// Obtiene si la aplicación está levantada
        /// </summary>
        /// <returns>Retorna código 200</returns>
        [HttpGet]
        [Route("live")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> Live()
        {
            return await Task.Run(() => { return Ok(true); });
        }

        /// <summary>
        /// Obtiene si la aplicación está preparada para ser consumida
        /// </summary>
        /// <returns>Retorna código 200 si está preparada para ser consumida, si no, retorna código 503</returns>
        [HttpGet]
        [Route("ready")]
        [ProducesResponseType(200)]
        [ProducesResponseType(503)]
        public async Task<ActionResult<bool>> Ready()
        {
            try
            {
                await GetRepository().ObtenerContador();
                return Ok(true);
            }
            catch (Exception)
            {
                return new ObjectResult(503);
            }
        }
    }
}