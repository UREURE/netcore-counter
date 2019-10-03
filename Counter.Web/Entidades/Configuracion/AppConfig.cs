namespace Counter.Web.Entidades.Configuracion
{
    internal class AppConfig : IAppConfig
    {
        public int? PuertoHttp { get; set; }
        public string Version { get; set; }
        public string Redis_Host { get; set; }
        public int? Redis_Port { get; set; }
        public string Redis_User { get; set; }
        public string Redis_Password { get; set; }
        public string Redis_Instance { get; set; }
    }
}