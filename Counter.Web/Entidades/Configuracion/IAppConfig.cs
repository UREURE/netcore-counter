namespace Counter.Web.Entidades.Configuracion
{
    internal interface IAppConfig
    {
        int? Counter_Http_Int_Port { get; set; }
        string Counter_Version { get; set; }
        string Redis_Host { get; set; }
        int? Redis_Port { get; set; }
        string Redis_Password { get; set; }
        string Redis_Instance { get; set; }
    }
}