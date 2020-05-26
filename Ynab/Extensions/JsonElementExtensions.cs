using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ynab.Extensions
{
    public static class JsonElementExtensions
    {
        public static bool TryGetProperty(this JsonElement source, IEnumerable<string> propertyNames, out JsonElement obj)
        {
            if (propertyNames == null || !propertyNames.Any())
                throw new ArgumentNullException(nameof(propertyNames));

            return InternalTryGetProperty(source, propertyNames, out obj);
        }

        private static bool InternalTryGetProperty(JsonElement source, IEnumerable<string> propertyNames, out JsonElement obj)
        {
            if (!propertyNames.Any())
            {
                obj = source;
                return true;
            }

            string propertyName = propertyNames.First();

            bool success = source.TryGetProperty(propertyName, out JsonElement subObj);
            if (success)
            {
                return InternalTryGetProperty(subObj, propertyNames.Skip(1), out obj);
            }

            obj = default;
            return false;
        }
    }
}
