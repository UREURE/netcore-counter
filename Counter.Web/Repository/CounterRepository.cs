using System.Threading.Tasks;

namespace Counter.Web.Repository
{
    internal class CounterRepository : ICounterRepository
    {
        public CounterRepository()
        {
            // TODO: Persistencia
            //this.dah = dah;
        }

        public async Task<int> ObtenerContador()
        {
            // TODO: Persistencia
            return await Task.Run(() => { return 1; });
        }

        public async Task<bool> IncrementarContador()
        {
            // TODO: Persistencia
            await Task.Run(() => { return; });
            return true;
        }
    }
}