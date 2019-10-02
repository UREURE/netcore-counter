namespace Counter.Web.Entidades.Configuracion
{
    internal class AppConfig : IAppConfig
    {
        public int? PuertoHttp { get; set; }
        public string CadenaConexion { get; set; }
        public string Version { get; set; }
    }
}