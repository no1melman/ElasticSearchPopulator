namespace ElasticSearchPopulator.Core.Extensions
{
    public static class StringExtensions
    {
        public static string InsertArgs(this string formatString, params object[] args)
        {
            return string.Format(formatString, args);
        }
    }
}