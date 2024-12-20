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

    public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> source, int count)
    {
        if (Equals(source, null))
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(count),
                "Count cannot be negative or equal zero."
            );
        }

        var sourceList = source.ToList();

        if (count > sourceList.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(count),
                "Count cannot be greater than the source length."
            );
        }

        // Shuffle the source list with OrderBy using random and return.
        var random = new Random();

        return sourceList.OrderBy(_ => random.Next()).Take(count);
    }
}
