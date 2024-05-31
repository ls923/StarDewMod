namespace StarDew_Mod_1.FrameWork.Core
{
    /// <summary>
    /// c# 单例
    /// </summary>
    public class Singleton<T> where T : new()
    {
        private static T singleton = default;
        private static readonly object _objectLock = new object();

        public static T Instance
        {
            get
            {
                if (singleton != null) return singleton;
                object obj;
                Monitor.Enter(obj = _objectLock); //加锁防止多线程创建单例
                try
                {
                    if (singleton == null)
                    {
                        singleton = default(T) == null ? Activator.CreateInstance<T>() : default; //创建单例的实例
                    }
                }
                finally
                {
                    Monitor.Exit(obj);
                }

                return singleton;
            }
        }

        public static void DestroyInstance()
        {
            //singleton = null;
        }
    }
}
