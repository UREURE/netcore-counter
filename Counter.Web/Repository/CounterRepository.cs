using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace Counter.Web.Repository
{
    internal class CounterRepository : ICounterRepository
    {
        private const string CLAVE_CONTADOR = "Counter";

        private readonly IDistributedCache distributedCache;

        public CounterRepository(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task<int> ObtenerContador()
        {
            string contador = await distributedCache.GetStringAsync(CLAVE_CONTADOR);
            if (string.IsNullOrEmpty(contador))
            {
                contador = "0";
                await distributedCache.SetStringAsync(CLAVE_CONTADOR, contador);
            }

            return int.Parse(contador);
        }

        public async Task<int> IncrementarContador()
        {
            int contador = await ObtenerContador();
            contador++;
            await distributedCache.SetStringAsync(CLAVE_CONTADOR, contador.ToString());
            return contador;
        }
    }
}