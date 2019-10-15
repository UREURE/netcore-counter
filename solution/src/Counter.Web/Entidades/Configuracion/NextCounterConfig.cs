namespace Counter.Web.Entidades.Configuracion
{
    internal class NextCounterConfig
    {
        public string Protocolo { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public int? Reintentos { get; set; }
    }
}