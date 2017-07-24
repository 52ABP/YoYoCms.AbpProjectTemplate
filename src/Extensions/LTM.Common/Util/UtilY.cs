using System;
using System.Collections.Generic;
using System.Threading;

namespace LTM.Common.Util
{
    /// <summary>
    ///     业务辅助类
    /// </summary>
    public static class UtilY
    {
        /// <summary>
        ///     进入业务处理
        /// </summary>
        /// <param name="identity">标识</param>
        /// <param name="seconds">默认为30秒. 30秒自动退出</param>
        /// <returns>
        ///     业务需要判断 如果返回false 则终止程序继续往下执行
        ///     只负责处理不允许二个人“同时调用”某个业务代码，业务需要处理“重复调用”逻辑。
        /// </returns>
        public static bool Enter(string identity)
        {
            return Enter(identity, 30);
        }

        /// <summary>
        ///     进入业务处理
        /// </summary>
        /// <param name="identity">标识</param>
        /// <param name="seconds">默认为30秒. 30秒自动退出</param>
        /// <returns>
        ///     业务需要判断 如果返回false 则终止程序继续往下执行
        ///     只负责处理不允许二个人“同时调用”某个业务代码，业务需要处理“重复调用”逻辑。
        /// </returns>
        public static bool Enter(string identity, int seconds)
        {
            return UtilYManager.Instance.Enter(identity, seconds);
        }

        /// <summary>
        ///     业务处理完毕
        /// </summary>
        /// <param name="identity">标识</param>
        public static void Exit(string identity)
        {
            UtilYManager.Instance.Exit(identity);
        }

        /// <summary>
        ///     清除内存中的数据
        /// </summary>
        public static void Clear()
        {
            UtilYManager.Instance.Clear();
        }
    }

    internal class UtilYManager
    {
        private readonly Dictionary<string, DateTime> _dict = new Dictionary<string, DateTime>();
        private readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();


        public bool Enter(string identity, int seconds)
        {
            _locker.EnterWriteLock();
            var flag = false;
            if (_dict.ContainsKey(identity))
            {
                if (_dict[identity].AddSeconds(seconds) < DateTime.Now)
                {
                    _dict[identity] = DateTime.Now;
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                _dict.Add(identity, DateTime.Now);
                flag = true;
            }
            _locker.ExitWriteLock();
            return flag;
        }

        public void Exit(string identity)
        {
            _locker.EnterWriteLock();
            if (_dict.ContainsKey(identity)) _dict.Remove(identity);
            _locker.ExitWriteLock();
        }

        public void Clear()
        {
            _dict.Clear();
        }

        #region singleton

        public static UtilYManager Instance
        {
            get { return Nested.Instance; }
        }

        private class Nested
        {
            internal static readonly UtilYManager Instance = new UtilYManager();

            static Nested()
            {
            }
        }

        #endregion
    }
}