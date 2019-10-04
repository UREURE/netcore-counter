using NLog;
using System;

namespace Counter.Web.Loggers
{
    internal class NLogLogger : ILogger
    {
        #region Constantes Públicas

        public const string NOMBRE_DEFECTO = "Log";

        #endregion

        #region Declaraciones

        protected readonly NLog.ILogger logger;

        #endregion

        #region Constructor

        public NLogLogger(string loggerName)
        {
            string nombre = string.IsNullOrEmpty(loggerName) ? NOMBRE_DEFECTO : loggerName;
            logger = LogManager.GetLogger(nombre);
        }

        public NLogLogger() : this(NOMBRE_DEFECTO)
        {
        }

        #endregion

        #region Metodos derivados de LoggerBase

        public virtual void Trace(string message)
        {
            logger.Trace(message);
        }

        public virtual void Debug(string message)
        {
            logger.Debug(message);
        }

        public virtual void Info(string message)
        {
            logger.Info(message);
        }

        public virtual void Warn(string message)
        {
            logger.Warn(message);
        }

        public virtual void Error(string message)
        {
            logger.Error(message);
        }

        public virtual void Error(Exception ex)
        {
            Error(ex.ToString());
        }

        public virtual void Fatal(string message)
        {
            logger.Fatal(message);
        }

        public virtual void Fatal(Exception ex)
        {
            Fatal(ex.ToString());
        }

        #endregion
    }
}