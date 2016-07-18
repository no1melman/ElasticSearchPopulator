namespace ElasticSearchPopulator.Core.Extensions
{
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
        public static string Join<T>(this IEnumerable<T> list, string separator)
        {
            return string.Join(separator, list);
        }

        public static string CommaSeparate<T>(this IEnumerable<T> list)
        {
            return list.Join(",");
        }
    }
}