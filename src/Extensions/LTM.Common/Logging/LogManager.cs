using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LTM.Common.Extensions;

namespace LTM.Common.Logging
{
    /// <summary>
    ///     日志管理器
    /// </summary>
    public static class LogManager
    {
        private static readonly ConcurrentDictionary<string, Logger> Loggers;
        private static readonly object LockObj = new object();

        static LogManager()
        {
            Loggers = new ConcurrentDictionary<string, Logger>();
            Adapters = new List<ILoggerAdapter>();
        }

        /// <summary>
        ///     获取 日志适配器集合
        /// </summary>
        internal static ICollection<ILoggerAdapter> Adapters { get; }

        /// <summary>
        ///     添加日志适配器
        /// </summary>
        public static void AddLoggerAdapter(ILoggerAdapter adapter)
        {
            lock (LockObj)
            {
                if (Adapters.Any(m => m == adapter))
                {
                    return;
                }
                Adapters.Add(adapter);
            }
        }

        /// <summary>
        ///     移除日志适配器
        /// </summary>
        public static void RemoveLoggerAdapter(ILoggerAdapter adapter)
        {
            lock (LockObj)
            {
                if (Adapters.All(m => m != adapter))
                {
                    return;
                }
                Adapters.Remove(adapter);
            }
        }

        /// <summary>
        ///     获取日志记录者实例
        /// </summary>
        public static Logger GetLogger(string name)
        {
            name.CheckNotNullOrEmpty(nameof(name));
            Logger logger;
            if (Loggers.TryGetValue(name, out logger))
            {
                return logger;
            }
            logger = new Logger(name);
            Loggers[name] = logger;
            return logger;
        }

        /// <summary>
        ///     获取指定类型的日志记录实例
        /// </summary>
        public static Logger GetLogger(Type type)
        {
            type.CheckNotNull(nameof(type));
            return GetLogger(type.FullName);
        }
    }
}