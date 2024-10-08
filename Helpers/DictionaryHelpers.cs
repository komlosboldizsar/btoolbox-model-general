﻿using System.Net.Http.Headers;

namespace BToolbox.Helpers
{
    public static class DictionaryHelpers
    {

        public static TValue GetAnyway<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> createDelegate)
        {
            if (!dictionary.TryGetValue(key, out TValue value))
            {
                value = createDelegate(key);
                dictionary.Add(key, value);
            }
            return value;
        }

        public static TValue GetAnyway<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
            => GetAnyway(dictionary, key, k => new TValue());

        public static void AddOrReplaceValues<TKey, TValue>(this Dictionary<TKey, TValue> changed, Dictionary<TKey, TValue> replacements)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in replacements)
                changed[kvp.Key] = kvp.Value;
        }

        public static Dictionary<TValue, TKey> ReverseKeysValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            Dictionary<TValue, TKey> result = new();
            foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
                result.Add(kvp.Value, kvp.Key);
            return result;
        }

    }
}
