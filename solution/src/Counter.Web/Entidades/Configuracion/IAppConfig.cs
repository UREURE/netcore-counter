namespace Counter.Web.Entidades.Configuracion
{
    internal interface IAppConfig
    {
        int? Counter_Http_Int_Port { get; set; }
        string Counter_Version { get; set; }
        RedisConfig Redis { get; set; }
    }
}