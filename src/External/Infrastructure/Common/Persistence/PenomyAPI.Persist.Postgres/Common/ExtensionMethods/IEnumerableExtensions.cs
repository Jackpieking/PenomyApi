using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PenomyAPI.Persist.Postgres.Common.ExtensionMethods;

internal static class IEnumerableExtensions
{
    private const string OPEN_BRACKET = "{";
    private const string CLOSE_BRACKET = "}";

    public static string ToSqlArray<T>(this IEnumerable<T> values)
        where T : IEquatable<T>, IComparable<T>
    {
        StringBuilder builder = new StringBuilder();

        int length = values.Count();
        for (int index = 0; index < length; index++)
        {
            var value = values.ElementAt(index);

            builder.Append(value);

            if (index < length - 1)
            {
                builder.Append(", ");
            }
        }

        return builder.ToString();
    }

    public static string ToSqlArray(this IEnumerable<string> values)
    {
        StringBuilder builder = new StringBuilder();

        // Add the open bracket at the beginning of the array.
        builder.Append(OPEN_BRACKET);

        int length = values.Count();
        for (int index = 0; index < length; index++)
        {
            var value = values.ElementAt(index);

            builder.Append($"'{value}'");

            if (index < length)
            {
                builder.Append(", ");
            }
        }

        // Add the close bracket at the end of the array.
        builder.Append(CLOSE_BRACKET);

        return builder.ToString();
    }
}
