namespace Counter.Web.Entidades.Configuracion
{
    internal interface IAppConfig
    {
        int? PuertoHttp { get; set; }
        string Version { get; set; }
        string Redis_Host { get; set; }
        int? Redis_Port { get; set; }
        string Redis_User { get; set; }
        string Redis_Password { get; set; }
        string Redis_Instance { get; set; }
    }
}