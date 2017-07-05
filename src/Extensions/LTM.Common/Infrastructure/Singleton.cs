using System;
using System.Linq;
using System.Reflection;

namespace LTM.Common.Infrastructure
{

    #region 问题版，无法保证线程安全

    //public sealed class Singleton
    //{
    //    private static Singleton instance;

    //    private Singleton() { }

    //    public static Singleton Instance
    //    {
    //        get
    //        {
    //            if (instance == null)
    //            {
    //                instance = new Singleton();
    //            }
    //            return instance;
    //        }
    //    }
    //}

    #endregion 问题版，无法保证线程安全

    #region sealed版本、无法保证创建的周期C#规范只是在IL里标记该静态字段是BeforeFieldInit，也就是说静态字段可能在第一次被使用的时候创建，也可能你没使用了，它也帮你创建了

    //public sealed class Singleton
    //{
    //    // 在静态私有字段上声明单例
    //    private static readonly Singleton instance = new Singleton();

    //    // 私有构造函数，确保用户在外部不能实例化新的实例
    //    private Singleton() { }

    //    // 只读属性返回静态字段
    //    public static Singleton Instance
    //    {
    //        get
    //        {
    //            return instance;
    //        }
    //    }
    //}

    #endregion sealed版本、无法保证创建的周期C#规范只是在IL里标记该静态字段是BeforeFieldInit，也就是说静态字段可能在第一次被使用的时候创建，也可能你没使用了，它也帮你创建了

    #region volatile修饰，确保instance在被访问之前被赋值实例，一般情况都是用这种方式来实现单例

    //public sealed class Singleton
    //{
    //    // 依然是静态自动hold实例
    //    private static volatile Singleton instance = null;
    //    // Lock对象，线程安全所用
    //    private static object syncRoot = new Object();

    //    private Singleton() { }

    //    public static Singleton Instance
    //    {
    //        get
    //        {
    //            if (instance == null)
    //            {
    //                lock (syncRoot)
    //                {
    //                    if (instance == null)
    //                        instance = new Singleton();
    //                }
    //            }

    //            return instance;
    //        }
    //    }
    //}

    #endregion volatile修饰，确保instance在被访问之前被赋值实例，一般情况都是用这种方式来实现单例

    #region 静态构造函数版、不错的选择，确实保证了是个延迟初始化的单例（通过加静态构造函数）

    //public class Singleton
    //{
    //    // 因为下面声明了静态构造函数，所以在第一次访问该类之前，new Singleton()语句不会执行
    //    private static readonly Singleton _instance = new Singleton();

    //    public static Singleton Instance
    //    {
    //        get { return _instance; }
    //    }

    //    private Singleton()
    //    {
    //    }

    //    // 声明静态构造函数就是为了删除IL里的BeforeFieldInit标记
    //    // 以去北欧静态自动在使用之前被初始化
    //    static Singleton()
    //    {
    //    }
    //}

    #endregion 静态构造函数版、不错的选择，确实保证了是个延迟初始化的单例（通过加静态构造函数）

    #region Lazy版，Lazy默认的设置就是线程安全

    //public class Singleton
    //{
    //    // 因为构造函数是私有的，所以需要使用lambda
    //    private static readonly Lazy<Singleton> _instance = new Lazy<Singleton>(() => new Singleton());
    //    // new Lazy<Singleton>(() => new Singleton(), LazyThreadSafetyMode.ExecutionAndPublication);

    //    private Singleton()
    //    {
    //    }

    //    public static Singleton Instance
    //    {
    //        get
    //        {
    //            return _instance.Value;
    //        }
    //    }
    //}

    #endregion Lazy版，Lazy默认的设置就是线程安全

    #region 泛型单例，存在问题为new

    //public class Singleton<T> where T : new()
    //{
    //    private static readonly Lazy<T> _instance
    //      = new Lazy<T>(() => new T());

    //    public static T Instance
    //    {
    //        get { return _instance.Value; }
    //    }
    //}

    #endregion 泛型单例，存在问题为new

    #region 泛型单列 终结版

    public abstract class Singleton<T>
    {
        private static readonly Lazy<T> _instance
            = new Lazy<T>(() =>
            {
                var ctors = typeof (T).GetConstructors(
                    BindingFlags.Instance
                    | BindingFlags.NonPublic
                    | BindingFlags.Public);
                if (ctors.Count() != 1)
                    throw new InvalidOperationException(string.Format("Type {0} must have exactly one constructor.",
                        typeof (T)));
                var ctor = ctors.SingleOrDefault(c => !c.GetParameters().Any() && c.IsPrivate);
                if (ctor == null)
                    throw new InvalidOperationException(
                        string.Format("The constructor for {0} must be private and take no parameters.", typeof (T)));
                return (T) ctor.Invoke(null);
            });

        public static T Instance
        {
            get { return _instance.Value; }
        }
    }

    public class MySingleton : Singleton<MySingleton>
    {
        private MySingleton()
        {
            Counter = 0;
        }

        public int Counter { get; private set; }

        public void IncrementCounter()
        {
            //Interlocked.Increment(ref _counter);
            ++Counter;
        }
    }

    public class Main
    {
        public void init()
        {
            var my = MySingleton.Instance;
        }
    }

    #endregion 泛型单列 终结版
}