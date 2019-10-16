namespace Counter.Web.Entidades.Configuracion
{
    /// <summary>
    ///
    /// </summary>
    public class NextCounterConfig
    {
        /// <summary>
        ///
        /// </summary>
        public string Protocolo { get; set; }

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
        public int? Reintentos { get; set; }
    }
}