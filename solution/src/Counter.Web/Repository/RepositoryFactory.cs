using System.Collections.Generic;

namespace Counter.Web.Repository
{
    internal class RepositoryFactory : IRepositoryFactory
    {
        private readonly Dictionary<string, ICounterRepository> diccionarioRepositorios;

        public RepositoryFactory()
        {
            diccionarioRepositorios = new Dictionary<string, ICounterRepository>();
        }

        public void Add(string clave, ICounterRepository repositorio)
        {
            diccionarioRepositorios.Add(clave, repositorio);
        }

        public ICounterRepository Get(string clave)
        {
            return diccionarioRepositorios[clave];
        }
    }
}