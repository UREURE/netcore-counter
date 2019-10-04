namespace Counter.Web.Entidades.Configuracion
{
    internal class RedisConfig
    {
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Password { get; set; }
        public string Instance { get; set; }
    }
}