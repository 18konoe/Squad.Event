using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SquadEvent.Shared.Utilities
{
    public interface IValueEquatable<in T>
    {
        bool ValueEquals(T other);
    }
    public static class Extends
    {
        public static bool IsEqualIgnoreCase(this string source, string compared)
        {
            return string.Compare(source, compared, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static string GetDiscordUserId(this IEnumerable<Claim> claims)
        {
            return claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        public static string GetDiscordEmail(this IEnumerable<Claim> claims)
        {
            return claims.First(c => c.Type == ClaimTypes.Email).Value;
        }

        public static bool ElementalEquals<T>(this IEnumerable<T> target, IEnumerable<T> other)
        {
            if (target == null) return other == null;
            if (other == null) return false;

            var targetArray = target as T[] ?? target.ToArray();
            var otherArray = other as T[] ?? other.ToArray();

            if (targetArray.Length != otherArray.Length) return false;
            return targetArray.All(otherArray.Contains) && otherArray.All(targetArray.Contains);
        }

        public static bool ValueEquals<TKey, TValue>(this IDictionary<TKey, TValue> target,
            IDictionary<TKey, TValue> other) where TValue : IValueEquatable<TValue>
        {
            if (target == null) return other == null;
            if (other == null) return false;

            if (target.Keys.Count != other.Keys.Count) return false;

            foreach (var pair in target)
            {
                if (!other.TryGetValue(pair.Key, out TValue otherValue)) return false;
                if (pair.Value == null && otherValue != null) return false;
                if (pair.Value != null && otherValue == null) return false;
                if (pair.Value != null && otherValue != null && !pair.Value.ValueEquals(otherValue)) return false;
            }

            return true;
        }

        public static bool ValueEquals<T>(this IEnumerable<T> target, IEnumerable<T> other) where T : IValueEquatable<T>
        {
            if (target == null) return other == null;
            if (other == null) return false;

            var targetArray = target as T[] ?? target.ToArray();
            var otherArray = other as T[] ?? other.ToArray();
            if (targetArray.Length != otherArray.Length) return false;

            return targetArray.All(otherArray.ValueContains) && otherArray.All(targetArray.ValueContains);
        }

        public static bool ValueContains<T>(this IEnumerable<T> target, T value) where T : IValueEquatable<T>
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (value == null) return target.Any(elem => elem == null);

            var valueEquatables = target as T[] ?? target.ToArray();
            return valueEquatables.Count() != 0 && valueEquatables.Any(elem => elem != null && elem.ValueEquals(value));
        }
    }
}