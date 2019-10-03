using Counter.Web.Constantes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Counter.Web.Controllers
{
    /// <summary>
    ///
    /// </summary>
    //[ApiController]
    [Route("/" + UriPath.PREFIX + "/[controller]")]
    public class HealthController : Controller
    {
        #region Constructores

        /// <summary>
        ///
        /// </summary>
        public HealthController()
        {
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("live")]
        public async Task<ActionResult<bool>> Live()
        {
            return await Task.Run(() => { return Ok(true); });
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ready")]
        public async Task<ActionResult<bool>> Ready()
        {
            return await Task.Run(() => { return Ok(true); });
        }
    }
}