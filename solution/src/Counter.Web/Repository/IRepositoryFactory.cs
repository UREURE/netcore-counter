namespace Counter.Web.Repository
{
    /// <summary>
    ///
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="clave"></param>
        /// <param name="repositorio"></param>
        void Add(string clave, ICounterRepository repositorio);

        /// <summary>
        ///
        /// </summary>
        /// <param name="clave"></param>
        /// <returns></returns>
        ICounterRepository Get(string clave);
    }
}