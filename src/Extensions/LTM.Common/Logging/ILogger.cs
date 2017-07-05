using System;

namespace LTM.Common.Logging
{
    /// <summary>
    ///     定义日志记录行为
    /// </summary>
    public interface ILogger
    {
        #region 方法

        /// <summary>
        ///     写入<see cref="LogLevel.Trace" />日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Trace<T>(T message);

        /// <summary>
        ///     写入<see cref="LogLevel.Trace" />格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Trace(string format, params object[] args);

        /// <summary>
        ///     写入<see cref="LogLevel.Debug" />日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Debug<T>(T message);

        /// <summary>
        ///     写入<see cref="LogLevel.Debug" />格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Debug(string format, params object[] args);

        /// <summary>
        ///     写入<see cref="LogLevel.Info" />日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Info<T>(T message);

        /// <summary>
        ///     写入<see cref="LogLevel.Info" />格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Info(string format, params object[] args);

        /// <summary>
        ///     写入<see cref="LogLevel.Warn" />日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Warn<T>(T message);

        /// <summary>
        ///     写入<see cref="LogLevel.Warn" />格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Warn(string format, params object[] args);

        /// <summary>
        ///     写入<see cref="LogLevel.Error" />日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Error<T>(T message);

        /// <summary>
        ///     写入<see cref="LogLevel.Error" />格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Error(string format, params object[] args);

        /// <summary>
        ///     写入<see cref="LogLevel.Error" />日志消息，并记录异常
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常</param>
        void Error<T>(T message, Exception exception);

        /// <summary>
        ///     写入<see cref="LogLevel.Error" />格式化日志消息，并记录异常
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="exception">异常</param>
        /// <param name="args">格式化参数</param>
        void Error(string format, Exception exception, params object[] args);

        /// <summary>
        ///     写入<see cref="LogLevel.Fatal" />日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Fatal<T>(T message);

        /// <summary>
        ///     写入<see cref="LogLevel.Fatal" />格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Fatal(string format, params object[] args);

        /// <summary>
        ///     写入<see cref="LogLevel.Fatal" />日志消息，并记录异常
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常</param>
        void Fatal<T>(T message, Exception exception);

        /// <summary>
        ///     写入<see cref="LogLevel.Fatal" />格式化日志消息，并记录异常
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="exception">异常</param>
        /// <param name="args">格式化参数</param>
        void Fatal(string format, Exception exception, params object[] args);

        #endregion 方法
    }
}