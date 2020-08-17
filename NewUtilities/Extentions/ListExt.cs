using System.Linq;

namespace System.Collections.Generic
{
    public static class ListExt
    {
        public static void Delete<T>(this List<T> list, Func<T, bool> condition, Action<T> deleteCallBack = null)
        {
            for(int i = list.Count - 1; i >= 0; i--)
            {
                if (condition.Invoke(list[i]))
                {
                    var item = list.ElementAt(i);
                    list.RemoveAt(i);
                    deleteCallBack?.Invoke(item);
                }
            }
        }
    }
}
