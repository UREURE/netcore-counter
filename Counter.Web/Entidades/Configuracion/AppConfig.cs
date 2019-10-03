namespace Counter.Web.Entidades.Configuracion
{
    internal class AppConfig : IAppConfig
    {
        public int? Counter_Http_Int_Port { get; set; }
        public string Counter_Version { get; set; }
        public string Redis_Host { get; set; }
        public int? Redis_Port { get; set; }
        public string Redis_Password { get; set; }
        public string Redis_Instance { get; set; }
    }
}