using Counter.Web.Constantes;
using Polly;
using Polly.Registry;
using System.Net.Http;
using System.Threading.Tasks;

namespace Counter.Web.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class NextCounterRepository : ICounterRepository
    {
        private const string NOMBRE_CONTROLADOR = "Counter";
        private const string NOMBRE_ACCION_LEER = "leer";
        private const string NOMBRE_ACCION_INCREMENTAR = "incrementar";

        private readonly IAsyncPolicy policy;
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="policyRegistry"></param>
        /// <param name="httpClientFactory"></param>
        public NextCounterRepository(IReadOnlyPolicyRegistry<string> policyRegistry, IHttpClientFactory httpClientFactory)
        {
            policy = policyRegistry.Get<IAsyncPolicy>(Claves.CLAVE_POLITICA_CACHE);
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task<string> LeerContadorNextCounter()
        {
            HttpClient httpClient = httpClientFactory.CreateClient(Claves.CLAVE_CLIENTE_HTTP_NEXT_COUNTER);
            string path = $"{UriPath.PREFIX}/{NOMBRE_CONTROLADOR}/{NOMBRE_ACCION_LEER}";
            string contador = await httpClient.GetStringAsync(path);
            return contador;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task<string> IncrementarContadorNextCounter()
        {
            HttpClient httpClient = httpClientFactory.CreateClient(Claves.CLAVE_CLIENTE_HTTP_NEXT_COUNTER);
            string path = $"{UriPath.PREFIX}/{NOMBRE_CONTROLADOR}/{NOMBRE_ACCION_INCREMENTAR}";
            string contador = await httpClient.GetStringAsync(path);
            return contador;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<int> ObtenerContador()
        {
            string contador = await policy.ExecuteAsync(() => LeerContadorNextCounter());
            return int.Parse(contador);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<int> IncrementarContador()
        {
            string contador = await policy.ExecuteAsync(() => IncrementarContadorNextCounter());
            return int.Parse(contador);
        }
    }
}