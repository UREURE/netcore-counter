namespace Counter.Web.Entidades.Configuracion
{
    internal interface IAppConfig
    {
        int? PuertoHttp { get; set; }
        string CadenaConexion { get; set; }
        string Version { get; set; }
    }
}