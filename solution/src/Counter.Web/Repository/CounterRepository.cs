using Counter.Web.Constantes;
using Microsoft.Extensions.Caching.Distributed;
using Polly;
using Polly.Registry;
using System.Threading.Tasks;

namespace Counter.Web.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class CounterRepository : ICounterRepository
    {
        private readonly ISyncPolicy policy;
        private readonly IDistributedCache distributedCache;

        /// <summary>
        ///
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="policyRegistry"></param>
        public CounterRepository(IDistributedCache distributedCache, IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            this.distributedCache = distributedCache;
            policy = policyRegistry.Get<ISyncPolicy>(Claves.CLAVE_POLITICA_CACHE);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task<string> ObtenerContadorCache()
        {
            string contador = await distributedCache.GetStringAsync(Claves.CLAVE_CONTADOR);
            if (string.IsNullOrEmpty(contador))
            {
                contador = "0";
                await distributedCache.SetStringAsync(Claves.CLAVE_CONTADOR, contador);
            }
            return contador;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="contador"></param>
        /// <returns></returns>
        protected async Task<string> GuardarContadorCache(string contador)
        {
            await distributedCache.SetStringAsync(Claves.CLAVE_CONTADOR, contador.ToString());
            return contador;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<int> ObtenerContador()
        {
            string contador = await policy.Execute(() => ObtenerContadorCache());
            return int.Parse(contador);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<int> IncrementarContador()
        {
            int contador = await ObtenerContador();
            contador++;
            await policy.Execute(() => GuardarContadorCache(contador.ToString()));
            return contador;
        }
    }
}