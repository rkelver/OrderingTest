using System.Collections.Generic;
using System.Linq;

namespace Common.Extension
{
    public static class EnumerableExtensions
    {
        public static IReadOnlyCollection<T> SetProperties<T>(this IReadOnlyCollection<T> collection,
            List<KeyValuePair<string, object>> dataToSet)
        {
            foreach (var item in collection)
            foreach (var prop in item.GetType().GetProperties())
            {
                var collectionProp = dataToSet.FirstOrDefault(l => l.Key == prop.Name);

                if (collectionProp.Key != null) prop.SetValue(item, collectionProp.Value);
            }

            return collection;
        }
    }
}