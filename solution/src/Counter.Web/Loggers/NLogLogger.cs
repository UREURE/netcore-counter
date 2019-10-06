using NLog;
using System;

namespace Counter.Web.Loggers
{
    internal class NLogLogger : ILogger
    {
        #region Declaraciones

        protected readonly NLog.ILogger logger;

        #endregion

        #region Constructor

        public NLogLogger()
        {
            logger = LogManager.GetCurrentClassLogger();
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