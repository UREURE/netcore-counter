using System.Threading.Tasks;

namespace Counter.Web.Repository
{
    /// <summary>
    ///
    /// </summary>
    public interface ICounterRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task<int> ObtenerContador();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task<bool> IncrementarContador();
    }
}