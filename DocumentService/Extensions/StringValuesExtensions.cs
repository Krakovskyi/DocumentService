namespace DocumentService.Extensions
{
    /// <summary>
    /// Extension methods for StringValues
    /// </summary>
    public static class StringValuesExtensions
    {
        /// <summary>
        /// Gets the first value or default from StringValues
        /// </summary>
        /// <param name="values">StringValues collection</param>
        /// <returns>First value or empty string</returns>
        public static string FirstValueOrDefault(this StringValues values)
        {
            return values.FirstOrDefault() ?? string.Empty;
        }
    }
} 