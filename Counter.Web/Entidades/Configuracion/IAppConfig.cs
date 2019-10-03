namespace Counter.Web.Entidades.Configuracion
{
    internal interface IAppConfig
    {
        int? PuertoHttp { get; set; }
        string Version { get; set; }
        string RedisHost { get; set; }
        int? RedisPort { get; set; }
        string RedisUser { get; set; }
        string RedisPassword { get; set; }
        string RedisInstance { get; set; }
    }
}