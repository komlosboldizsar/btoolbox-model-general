using System.Collections;

namespace BToolbox.Helpers
{

    public class EnumConverter<TEnum, TConvertedValue>: IEnumerable<KeyValuePair<TEnum, TConvertedValue>>
    {

        private readonly Dictionary<TEnum, TConvertedValue> convertedValues = new();
        private readonly TConvertedValue defaultValue;

        public EnumConverter(TConvertedValue defaultValue = default, Dictionary <TEnum, TConvertedValue> convertedValues = null)
        {
            this.defaultValue = defaultValue;
            if (convertedValues != null)
                foreach (var item in convertedValues)
                    this.convertedValues.Add(item.Key, item.Value);
        }

        public TConvertedValue Convert(TEnum enumValue)
        {
            if (!convertedValues.TryGetValue(enumValue, out TConvertedValue convertedValue))
                return defaultValue;
            return convertedValue;
        }

        public IEnumerable<TEnum> EnumValues => convertedValues.Keys;
        public IEnumerable<TConvertedValue> ConvertedValues => convertedValues.Values;

        #region Implement IEnumerable, so we can use collection initializers
        public IEnumerator<KeyValuePair<TEnum, TConvertedValue>> GetEnumerator()
            => convertedValues.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => convertedValues.GetEnumerator();

        public void Add(TEnum enumValue, TConvertedValue convertedValue)
            => convertedValues.Add(enumValue, convertedValue);
        #endregion

    }

}
