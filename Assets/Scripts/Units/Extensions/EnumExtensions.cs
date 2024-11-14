using System;
using System.Linq;
using PlayVibe;
using UnityEngine;
using Random = System.Random;

namespace Units.Extensions
{
    public static class EnumExtensions
    {
        private static readonly Random random = new();

        public static T GetRandomValue<T>() where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return values[random.Next(values.Length)];
        }

        public static T GetRandomValueExcluding<T>(T exclude) where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            var filteredValues = values.Where(value => !value.Equals(exclude)).ToArray();

            if (filteredValues.Length == 0)
            {
                throw new InvalidOperationException("No values left to select from after exclusion.".AddColorTag(Color.red));
            }

            return filteredValues[random.Next(filteredValues.Length)];
        }
    }
}