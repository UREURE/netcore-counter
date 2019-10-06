using System;

namespace Counter.Web.Loggers
{
    /// <summary>
    ///
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        void Trace(string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        void Warn(string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        void Fatal(string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ex"></param>
        void Fatal(Exception ex);
    }
}