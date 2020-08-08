namespace Utilities.Singleton
{
    public class Singleton<T> where T : class
    {
        private static T instance;
        private static readonly object locker = new object();
        public static T GetInstance()
        {
            if(instance == null)
            {
                lock(locker)
                {
                    if (instance == null)
                        instance = default;
                }
            }

            return instance;
        }
    }
}
