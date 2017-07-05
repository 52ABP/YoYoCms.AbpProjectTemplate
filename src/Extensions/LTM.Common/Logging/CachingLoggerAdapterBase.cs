using System;
using System.Collections.Concurrent;
using LTM.Common.Extensions;
using LTM.Common.Properties;

namespace LTM.Common.Logging
{
    /// <summary>
    ///     按名称缓存的日志实现适配器基类，用于创建并管理指定类型的日志实例
    /// </summary>
    public abstract class CachingLoggerAdapterBase : ILoggerAdapter
    {
        private readonly ConcurrentDictionary<string, ILog> _cacheLoggers;

        /// <summary>
        ///     初始化一个<see cref="CachingLoggerAdapterBase" />类型的新实例
        /// </summary>
        protected CachingLoggerAdapterBase()
        {
            _cacheLoggers = new ConcurrentDictionary<string, ILog>();
        }

        /// <summary>
        ///     创建指定名称的缓存实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        protected abstract ILog CreateLogger(string name);

        /// <summary>
        ///     清除缓存中的日志实例
        /// </summary>
        protected virtual void ClearLoggerCache()
        {
            _cacheLoggers.Clear();
        }

        private ILog GetLoggerInternal(string name)
        {
            ILog log;
            if (_cacheLoggers.TryGetValue(name, out log))
            {
                return log;
            }
            log = CreateLogger(name);
            if (log == null)
            {
                throw new NotSupportedException(Resources.Logging_CreateLogInstanceReturnNull.FormatWith(name,
                    GetType().FullName));
            }
            _cacheLoggers[name] = log;
            return log;
        }

        #region Implementation of ILoggerFactoryAdapter

        /// <summary>
        ///     由指定类型获取<see cref="ILog" />日志实例
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        public ILog GetLogger(Type type)
        {
            type.CheckNotNull(nameof(type));
            return GetLoggerInternal(type.FullName);
        }

        /// <summary>
        ///     由指定名称获取<see cref="ILog" />日志实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        public ILog GetLogger(string name)
        {
            name.CheckNotNullOrEmpty(nameof(name));
            return GetLoggerInternal(name);
        }

        #endregion Implementation of ILoggerFactoryAdapter
    }
}