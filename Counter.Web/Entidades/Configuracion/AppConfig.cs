namespace Counter.Web.Entidades.Configuracion
{
    internal class AppConfig : IAppConfig
    {
        public int? PuertoHttp { get; set; }
        public string CadenaConexion { get; set; }
        public string Version { get; set; }
        public string RedisHost { get; set; }
        public int? RedisPort { get; set; }
        public string RedisUser { get; set; }
        public string RedisPassword { get; set; }
    }
}