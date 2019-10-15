namespace Counter.Web.Entidades.Configuracion
{
    internal class AppConfig : IAppConfig
    {
        public int? Counter_Http_Int_Port { get; set; }
        public string Counter_Version { get; set; }
        public RedisConfig Redis { get; set; }
        public NextCounterConfig NextCounter { get; set; }
        public string Counter_Selector_Persistencia { get; set; }
    }
}