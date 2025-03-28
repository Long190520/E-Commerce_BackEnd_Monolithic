namespace E_Commerce_BackEnd.Utils
{
    public static class IEnumerableUtils
    {
        public static async Task<IEnumerable<T>> ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
        {
            if (source.Nothing() || action == null)
            {
                return source;
            }

            IEnumerable<Task> tasks = source.Select(action);
            await Task.WhenAll(tasks);
            return source;
        }

        public static bool Nothing<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }
    }
}
