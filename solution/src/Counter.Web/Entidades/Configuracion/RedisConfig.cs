namespace Counter.Web.Entidades.Configuracion
{
    /// <summary>
    ///
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        ///
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int? Reintentos { get; set; }
    }
}