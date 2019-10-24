namespace Counter.Web.Entidades.Configuracion
{
    /// <summary>
    ///
    /// </summary>
    public class AppConfig : IAppConfig
    {
        /// <summary>
        ///
        /// </summary>
        public int? Counter_Http_Int_Port { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Counter_Version { get; set; }

        /// <summary>
        ///
        /// </summary>
        public RedisConfig Redis { get; set; }

        /// <summary>
        ///
        /// </summary>
        public NextCounterConfig NextCounter { get; set; }

        /// <summary>
        ///
        /// </summary>
        public FeatureManagementConfig FeatureManagement { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsFeaturePersistenciaNextCounterEnabled
        {
            get
            {
                return FeatureManagement != null &&
                    FeatureManagement.PersistenciaNextCounter.HasValue &&
                    FeatureManagement.PersistenciaNextCounter.Value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="activada"></param>
        public void SetFeaturePersistenciaNextCounterEnabled(bool activada)
        {
            if (FeatureManagement == null)
                FeatureManagement = new FeatureManagementConfig();
            FeatureManagement.PersistenciaNextCounter = activada;
        }
    }
}